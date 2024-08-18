using BankDemo.Domain.Entities.Identity;
using BankDemo.Infrastructure.Exceptions;
using BankDemo.Infrastructure.Extensions;
using BankDemo.Infrastructure.Services.Interfaces;
using BankDemo.Infrastructure.Settings;
using BankDemo.Infrastructure.Wrappers;
using BankDemo.Shared.DTOs.Identity.Requests;
using BankDemo.Shared.DTOs.Identity.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BankDemo.Infrastructure.Services;

public class TokenService(
    UserManager<ApplicationUser> userManager,
    JwtSettings config) : ITokenService
{
    public async Task<IResult<TokenResponse>> GetTokenAsync(TokenRequest request)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(x => x.Email.Equals(request.Email)) ??
            throw new IdentityException("User not found.");

        bool passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid) throw new IdentityException("Incorrect login and/or password.");

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(config.RefreshTokenExpirationInDays);
        await userManager.UpdateAsync(user);
        string token = await GenerateJwtAsync(user);
        var response = new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
        return await Result<TokenResponse>.SuccessAsync(response);
    }

    public async Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        if (request is null) throw new IdentityException("Invalid token.");

        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        var userId = userPrincipal.GetUserId();
        var user = await userManager.FindByIdAsync(userId) ??
            throw new IdentityException("User not found.");

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new IdentityException("Invalid token.");

        string token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(config.RefreshTokenExpirationInDays);
        await userManager.UpdateAsync(user);
        var response = new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);

        return await Result<TokenResponse>.SuccessAsync(response);
    }

    private async Task<string> GenerateJwtAsync(ApplicationUser user)
    {
        return GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new("email", user.Email)
        };

        return claims;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(config.TokenExpirationInMinutes),
            signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = default(SecurityToken);
        var principal = default(ClaimsPrincipal);
        principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new IdentityException("Invalid token.");

        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(config.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
}