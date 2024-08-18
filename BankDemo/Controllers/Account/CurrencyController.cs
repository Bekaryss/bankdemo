using BankDemo.Core.Interfaces;
using BankDemo.Infrastructure.Wrappers;
using BankDemo.Shared.DTOs.Currency;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BankDemo.Controllers.Account
{
    [ApiController]
    [Authorize]
    [Route("api/user/accounts/currency")]
    public class CurrencyControllerController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly ILogger<CurrencyControllerController> _logger;

        public CurrencyControllerController(ICurrencyService currencyService, ILogger<CurrencyControllerController> logger)
        {
            _currencyService = currencyService;
            _logger = logger;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IResult<CurrencyDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAccount(string id, CancellationToken ct)
        {
            try
            {
                var guidId = Guid.Parse(id);
                var response = await _currencyService.Get(guidId, ct);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IResult<CurrencyDto[]>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAccounts(CancellationToken ct)
        {
            try
            {
                var response = await _currencyService.GetAll(ct);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
