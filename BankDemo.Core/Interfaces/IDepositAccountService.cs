using BankDemo.Infrastructure.Wrappers;
using BankDemo.Shared.DTOs.Account;
using BankDemo.Shared.DTOs.Account.Requests;

namespace BankDemo.Core.Interfaces
{
    public interface IDepositAccountService : IAccountService
    {
        Task<IResult<DepositAccountDto>> GetAccount(Guid id, CancellationToken cancellationToken);
        Task<IResult<DepositAccountDto[]>> GetAccounts(string userId, CancellationToken cancellationToken);
        Task<IResult<DepositAccountDto>> CreateAccount(CreateDepositAccountRequest request, string UserId, CancellationToken cancellationToken);
    }
}
