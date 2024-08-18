using BankDemo.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;

namespace BankDemo.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions options) : BaseDbContext(options)
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<DefaultAccount> DefaultAccounts { get; set; }
        public DbSet<DepositAccount> DepositAccounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasDiscriminator(x => x.AccountType)
                .HasValue<Account>(AccountType.General)
                .HasValue<DefaultAccount>(AccountType.Default)
                .HasValue<DepositAccount>(AccountType.Deposit);

            modelBuilder.Entity<Account>()
                .Property(a => a.AccountNumber)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Account>()
                .Property(a => a.OwnerId)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        public async Task SeedCurrenciesAsync()
        {
            if (!Currencies.Any())
            {
                var currencies = new List<Currency>
            {
                new Currency
                {
                    Name = "United States Dollar",
                    ShortSign = "USD",
                    Symbol = "$",
                    Description = "US Dollar"
                },
                new Currency
                {
                    Name = "Euro",
                    ShortSign = "EUR",
                    Symbol = "€",
                    Description = "Euro"
                },
                new Currency
                {
                    Name = "Kazakhstani Tenge",
                    ShortSign = "KZT",
                    Symbol = "₸",
                    Description = "Kazakhstan Tenge"
                }
            };

                Currencies.AddRange(currencies);
                await SaveChangesAsync();
            }
        }
    }
}
