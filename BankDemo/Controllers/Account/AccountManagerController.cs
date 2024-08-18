using BankDemo.Core.Interfaces;
using BankDemo.Shared.DTOs.Account.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankDemo.Controllers.Account
{
    [ApiController]
    [Authorize]
    [Route("api/user/accounts/manager")]
    public class AccountManagerController : ControllerBase
    {
        private readonly IAccountManagerService _accountManagerService;
        private readonly ILogger<AccountManagerController> _logger;

        public AccountManagerController(IAccountManagerService accountManagerService, ILogger<AccountManagerController> logger)
        {
            _accountManagerService = accountManagerService;
            _logger = logger;
        }

        [HttpPost("deposit/{id}")]
        public async Task<IActionResult> Deposit(string id, [FromBody] decimal amount, CancellationToken ct)
        {
            try
            {
                var guidId = Guid.Parse(id);
                var response = await _accountManagerService.DepositAsync(guidId, amount, ct);

                if (response.Succeeded)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during deposit.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("withdraw/{id}")]
        public async Task<IActionResult> Withdraw(string id, [FromBody] decimal amount, CancellationToken ct)
        {
            try
            {
                var guidId = Guid.Parse(id);
                var response = await _accountManagerService.WithdrawalAsync(guidId, amount, ct);

                if (response.Succeeded)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during withdrawal.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest request, CancellationToken ct)
        {
            try
            {
                var response = await _accountManagerService.TransferAsync(request.FromAccountId, request.ToAccountId, request.Amount, ct);

                if (response.Succeeded)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during transfer.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
