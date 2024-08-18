using BankDemo.Infrastructure.Exceptions;
using BankDemo.Infrastructure.Services.Interfaces;
using BankDemo.Shared.DTOs.Identity.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace BankDemo.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public sealed class TokensController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<TokensController> _logger;

    public TokensController(ITokenService tokenService, ILogger<TokensController> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> GetTokenAsync(TokenRequest request)
    {
        try
        {
            var token = await _tokenService.GetTokenAsync(request);

            return Ok(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }

    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshAsync(RefreshTokenRequest request)
    {
        try
        {
            var response = await _tokenService.RefreshTokenAsync(request);

            return Ok(response);
        }
        catch (SecurityTokenExpiredException ex)
        {
            throw new IdentityException(
                "You need to log in again!", statusCode: HttpStatusCode.Unauthorized);
        }
    }
}