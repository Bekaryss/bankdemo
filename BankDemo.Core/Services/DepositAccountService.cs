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
    public class DepositAccountService : IDepositAccountService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DefaultAccountService> _logger;

        public DepositAccountService(ApplicationDbContext dbContext, ILogger<DefaultAccountService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IResult<DepositAccountDto>> CreateAccount(CreateDepositAccountRequest request, string userId, CancellationToken cancellationToken)
        {
            var account = new DepositAccount()
            {
                CurrencyId = request.CurrencyId,
                AccountType = AccountType.Deposit,
                Balance = 0,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Locked = false,
                Deleted = false,
                Name = request.AccountName,
                AccountNumber = Guid.NewGuid().ToString(),
                OwnerId = userId,
                Percentage = request.DepositPercentage,
                ExpireTime = DateTime.UtcNow.AddMonths(request.DepositLifeTimeInMounth)
            };

            _dbContext.DepositAccounts.Add(account);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<DepositAccountDto>.Success(account.Adapt<DepositAccountDto>());
        }

        public async Task<IResult<DepositAccountDto>> GetAccount(Guid id, CancellationToken cancellationToken)
        {
            var account = await _dbContext.DepositAccounts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (account == null)
            {
                return Result<DepositAccountDto>.Fail("Cannot find any account.");
            }

            return Result<DepositAccountDto>.Success(account.Adapt<DepositAccountDto>());
        }

        public async Task<IResult<DepositAccountDto[]>> GetAccounts(string userId, CancellationToken cancellationToken)
        {
            var accounts = await _dbContext.DepositAccounts.Where(x => x.OwnerId == userId).ToArrayAsync(cancellationToken);

            return Result<DepositAccountDto[]>.Success(accounts.Adapt<DepositAccountDto[]>());
        }

        public async Task<IResult> Lock(Guid id, string userId, CancellationToken cancellationToken)
        {
            var account = await _dbContext.DepositAccounts.FirstOrDefaultAsync(x => x.Id == id && x.OwnerId == userId, cancellationToken);
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
