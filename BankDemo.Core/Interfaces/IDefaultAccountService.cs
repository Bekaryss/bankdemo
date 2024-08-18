using BankDemo.Infrastructure.Wrappers;
using BankDemo.Shared.DTOs.Account;
using BankDemo.Shared.DTOs.Account.Requests;

namespace BankDemo.Core.Interfaces
{
    public interface IDefaultAccountService : IAccountService
    {
        Task<IResult<DefaultAccountDto>> GetAccount(Guid id, CancellationToken cancellationToken);
        Task<IResult<DefaultAccountDto[]>> GetAccounts(string userId, CancellationToken cancellationToken);
        Task<IResult<DefaultAccountDto>> CreateAccount(CreateDefaultAccountRequest request, string userId, CancellationToken cancellationToken);
    }
}
