using BankDemo.Infrastructure.Wrappers;

namespace BankDemo.Core.Interfaces
{
    public interface IAccountService
    {
        Task<IResult> Lock(Guid Id, string UserId, CancellationToken cancellationToken);
    }
}
