using BankDemo.Core.Interfaces;
using BankDemo.Infrastructure.Persistence;
using BankDemo.Infrastructure.Wrappers;
using BankDemo.Shared.DTOs.Currency;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankDemo.Core.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(ApplicationDbContext dbContext, ILogger<CurrencyService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<IResult<CurrencyDto>> Get(Guid Id, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _dbContext.Currencies.FirstOrDefaultAsync(x => x.Id == Id, cancellationToken);
                if (item == null)
                {
                    return Result<CurrencyDto>.Fail("Cannot find any currency.");
                }
                var result = item.Adapt<CurrencyDto>();

                return Result<CurrencyDto>.Success(result);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                throw;
            }
        }

        public async Task<IResult<CurrencyDto[]>> GetAll(CancellationToken cancellationToken)
        {
            var items = await _dbContext.Currencies.ToArrayAsync(cancellationToken);

            return Result<CurrencyDto[]>.Success(items.Adapt<CurrencyDto[]>());
        }
    }
}
