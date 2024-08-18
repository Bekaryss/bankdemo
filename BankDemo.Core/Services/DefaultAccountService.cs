using BankDemo.Core.Interfaces;
using BankDemo.Domain.Entities.Account;
using BankDemo.Infrastructure.Persistence;
using BankDemo.Infrastructure.Wrappers;
using BankDemo.Shared.DTOs.Account;
using BankDemo.Shared.DTOs.Account.Requests;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankDemo.Core.Services
{
    public class DefaultAccountService : IDefaultAccountService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DefaultAccountService> _logger;

        public DefaultAccountService(ApplicationDbContext dbContext, ILogger<DefaultAccountService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IResult<DefaultAccountDto>> CreateAccount(CreateDefaultAccountRequest request, string userId, CancellationToken cancellationToken)
        {
            var account = new DefaultAccount()
            {
                CurrencyId = request.CurrencyId,
                AccountType = AccountType.Default,
                Balance = 0,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Locked = false,
                Deleted = false,
                Name = request.AccountName,
                AccountNumber = Guid.NewGuid().ToString(),
                OwnerId = userId,
            };
            if (!_dbContext.DefaultAccounts.Any(x => x.OwnerId == userId))
            {
                account.IsMain = true;
            }

            _dbContext.DefaultAccounts.Add(account);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<DefaultAccountDto>.Success(account.Adapt<DefaultAccountDto>());
        }

        public async Task<IResult<DefaultAccountDto>> GetAccount(Guid id, CancellationToken cancellationToken)
        {
            var account = await _dbContext.DefaultAccounts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (account == null)
            {
                return Result<DefaultAccountDto>.Fail("Cannot find any account.");
            }

            return Result<DefaultAccountDto>.Success(account.Adapt<DefaultAccountDto>());
        }

        public async Task<IResult<DefaultAccountDto[]>> GetAccounts(string userId, CancellationToken cancellationToken)
        {
            var accounts = await _dbContext.DefaultAccounts.Where(x => x.OwnerId == userId).ToArrayAsync(cancellationToken);

            return Result<DefaultAccountDto[]>.Success(accounts.Adapt<DefaultAccountDto[]>());
        }

        public async Task<IResult> Lock(Guid id, string userId, CancellationToken cancellationToken)
        {
            var account = await _dbContext.DefaultAccounts.FirstOrDefaultAsync(x => x.Id == id && x.OwnerId == userId, cancellationToken);
            if (account == null)
            {
                return Result.Fail("Cannot find the account or you do not have access to it.");
            }

            account.Locked = true;
            account.LockBy = userId;
            _dbContext.Update(account);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success("Locked!");
        }
    }
}
