using BankDemo.Core.Interfaces;
using BankDemo.Infrastructure.Services.Interfaces;
using BankDemo.Infrastructure.Wrappers;
using BankDemo.Shared.DTOs.Account;
using BankDemo.Shared.DTOs.Account.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BankDemo.Controllers.Account
{
    [ApiController]
    [Authorize]
    [Route("api/user/accounts/deposit")]
    public class DepositAccountController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDepositAccountService _accountService;
        private readonly ILogger<DepositAccountController> _logger;

        public DepositAccountController(ICurrentUserService currentUserService, IDepositAccountService accountService, ILogger<DepositAccountController> logger)
        {
            _currentUserService = currentUserService;
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(IResult<DepositAccountDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDepositAccount([FromBody] CreateDepositAccountRequest rq, CancellationToken ct)
        {
            try
            {
                var currentUserId = _currentUserService.GetUserId().ToString();
                var response = await _accountService.CreateAccount(rq, currentUserId, ct);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{userId}/{id}")]
        [ProducesResponseType(typeof(IResult<DepositAccountDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAccount(string userId, string id, CancellationToken ct)
        {
            try
            {
                var guidId = Guid.Parse(id);
                var currentUserId = _currentUserService.GetUserId().ToString();
                if (currentUserId != userId)
                {
                    return BadRequest("This user do not have access.");
                }
                var response = await _accountService.GetAccount(guidId, ct);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(IResult<DepositAccountDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAccounts(string userId, CancellationToken ct)
        {
            try
            {
                var currentUserId = _currentUserService.GetUserId().ToString();
                if (currentUserId != userId)
                {
                    return BadRequest("This user do not have access.");
                }
                var response = await _accountService.GetAccounts(userId, ct);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("lock/{userId}/{id}")]
        public async Task<IActionResult> Lock(string userId, string id, CancellationToken ct)
        {
            try
            {
                var guidId = Guid.Parse(id);
                var currentUserId = _currentUserService.GetUserId().ToString();
                if (currentUserId != userId)
                {
                    return BadRequest("This user do not have access.");
                }
                var response = await _accountService.Lock(guidId, currentUserId, ct);

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
