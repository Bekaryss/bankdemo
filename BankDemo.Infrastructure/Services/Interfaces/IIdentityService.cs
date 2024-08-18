using BankDemo.Infrastructure.Wrappers;
using BankDemo.Shared.DTOs.Identity;
using BankDemo.Shared.DTOs.Identity.Requests;

namespace BankDemo.Infrastructure.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<IResult<UserDetailsDto>> GetUserAsync(string userId);
        Task<IResult> RegisterAsync(RegisterRequest request);
    }
}
