using BankDemo.Infrastructure.Wrappers;
using BankDemo.Shared.DTOs.Identity.Requests;
using BankDemo.Shared.DTOs.Identity.Responses;

namespace BankDemo.Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        Task<IResult<TokenResponse>> GetTokenAsync(TokenRequest request);

        Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
