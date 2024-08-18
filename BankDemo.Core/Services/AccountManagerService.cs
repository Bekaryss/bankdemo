using BankDemo.Core.Interfaces;
using BankDemo.Infrastructure.Persistence;
using BankDemo.Infrastructure.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace BankDemo.Core.Services
{
    public class AccountManagerService : IAccountManagerService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AccountManagerService> _logger;

        public AccountManagerService(ApplicationDbContext dbContext, ILogger<AccountManagerService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IResult> DepositAsync(Guid accountId, decimal amount, CancellationToken cancellationToken)
        {
            if (amount <= 0)
            {
                return Result.Fail("Deposit amount must be greater than zero.");
            }

            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    // Retrieve the account
                    var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
                    if (account == null)
                    {
                        return Result.Fail("Account not found.");
                    }

                    if (account.Locked)
                    {
                        return Result.Fail("Account is locked and cannot receive deposits.");
                    }

                    account.Balance += amount;
                    _dbContext.Update(account);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Result.Success("Deposit Successful!");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);

                    _logger.LogError(ex.Message);
                    throw;
                }
            }
        }
        public async Task<IResult> WithdrawalAsync(Guid accountId, decimal amount, CancellationToken cancellationToken)
        {
            if (amount <= 0)
            {
                return Result.Fail("Withdrawal amount must be greater than zero.");
            }

            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    // Retrieve the account
                    var account = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);
                    if (account == null)
                    {
                        return Result.Fail("Account not found.");
                    }

                    if (account.Locked)
                    {
                        return Result.Fail("Account is locked and cannot process withdrawals.");
                    }

                    if (account.Balance < amount)
                    {
                        return Result.Fail("Insufficient funds for the withdrawal.");
                    }

                    account.Balance -= amount;
                    _dbContext.Update(account);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    return Result.Success("Withdrawal successful.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    _logger.LogError(ex.Message);
                    throw;
                }
            }
        }


        public async Task<IResult> TransferAsync(Guid fromAccountId, Guid toAccountId, decimal amount, CancellationToken cancellationToken)
        {
            if (amount <= 0)
            {
                return Result.Fail("Transfer amount must be greater than zero.");
            }

            if (fromAccountId == toAccountId)
            {
                return Result.Fail("Cannot transfer to the same account.");
            }

            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var fromAccount = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == fromAccountId, cancellationToken);
                    if (fromAccount == null)
                    {
                        return Result.Fail("Source account not found.");
                    }

                    var toAccount = await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == toAccountId, cancellationToken);
                    if (toAccount == null)
                    {
                        return Result.Fail("Destination account not found.");
                    }

                    if (fromAccount.Locked)
                    {
                        return Result.Fail("Source account is locked and cannot process withdrawals.");
                    }

                    if (toAccount.Locked)
                    {
                        return Result.Fail("Destination account is locked and cannot receive deposits.");
                    }

                    if (fromAccount.Balance < amount)
                    {
                        return Result.Fail("Insufficient funds in the source account.");
                    }

                    decimal convertedAmount = amount;
                    if (fromAccount.CurrencyId != toAccount.CurrencyId)
                    {
                        var exchangeRate = await GetExchangeRateAsync(fromAccount.CurrencyId, toAccount.CurrencyId, cancellationToken);
                        if (exchangeRate == null)
                        {
                            return Result.Fail("Unable to retrieve exchange rate for the currencies.");
                        }

                        convertedAmount = amount * exchangeRate.Value;
                    }

                    fromAccount.Balance -= amount;
                    toAccount.Balance += convertedAmount;

                    _dbContext.Update(fromAccount);
                    _dbContext.Update(toAccount);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);

                    return Result.Success("Transfer successful.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task<decimal?> GetExchangeRateAsync(Guid fromCurrencyId, Guid toCurrencyId, CancellationToken cancellationToken)
        {
            var fromCurrency = await _dbContext.Currencies.FirstOrDefaultAsync(x => x.Id == fromCurrencyId, cancellationToken);
            var toCurrency = await _dbContext.Currencies.FirstOrDefaultAsync(x => x.Id == toCurrencyId, cancellationToken);

            if (string.IsNullOrEmpty(fromCurrency?.ShortSign) || string.IsNullOrEmpty(fromCurrency?.ShortSign))
            {
                return null;
            }

            var exchangeRates = new Dictionary<(string, string), decimal>
            {
                { ("USD", "EUR"), 0.85m },  // 1 USD = 0.85 EUR
                { ("EUR", "USD"), 1.18m },  // 1 EUR = 1.18 USD
                { ("USD", "KZT"), 420m },   // 1 USD = 420 KZT
                { ("KZT", "USD"), 0.00238m }, // 1 KZT = 0.00238 USD
                { ("EUR", "KZT"), 495m },   // 1 EUR = 495 KZT
                { ("KZT", "EUR"), 0.00202m }  // 1 KZT = 0.00202 EUR
            };

            if (exchangeRates.TryGetValue((fromCurrency.ShortSign, toCurrency.ShortSign), out decimal rate))
            {
                return rate;
            }

            return null;
        }
    }
}
