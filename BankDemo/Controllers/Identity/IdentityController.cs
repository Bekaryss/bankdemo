using BankDemo.Infrastructure.Services.Interfaces;
using BankDemo.Shared.DTOs.Identity.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankDemo.Controllers.Identity;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public sealed class IdentityController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(IIdentityService identityService, ILogger<IdentityController> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");
            var result = await _identityService.RegisterAsync(request);
            if (!result.Succeeded)
            {
                return BadRequest(result.Messages);
            }
            _logger.LogInformation("Registration Successful! Request: {@request}", request);

            return Ok("Registration Successful!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}