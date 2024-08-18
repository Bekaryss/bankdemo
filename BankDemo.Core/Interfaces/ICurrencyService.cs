using BankDemo.Infrastructure.Wrappers;
using BankDemo.Shared.DTOs.Currency;

namespace BankDemo.Core.Interfaces
{
    public interface ICurrencyService
    {
        Task<IResult<CurrencyDto>> Get(Guid Id, CancellationToken cancellationToken);
        Task<IResult<CurrencyDto[]>> GetAll(CancellationToken cancellationToken);
    }
}
