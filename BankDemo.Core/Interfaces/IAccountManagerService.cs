using BankDemo.Infrastructure.Wrappers;

namespace BankDemo.Core.Interfaces
{
    public interface IAccountManagerService
    {
        Task<IResult> DepositAsync(Guid accountId, decimal amount, CancellationToken cancellationToken);
        Task<IResult> WithdrawalAsync(Guid accountId, decimal amount, CancellationToken cancellationToken);
        Task<IResult> TransferAsync(Guid fromAccountId, Guid toAccountId, decimal amount, CancellationToken cancellationToken);
    }
}
