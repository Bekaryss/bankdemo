using BankDemo.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Astra.Bootstrapper.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ProfileController : ControllerBase
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IIdentityService identityService, ICurrentUserService currentUserService, ILogger<ProfileController> logger)
    {
        _identityService = identityService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfileAsync()
    {
        try
        {
            var currentUserId = _currentUserService.GetUserId().ToString();

            return Ok(await _identityService.GetUserAsync(currentUserId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }
}