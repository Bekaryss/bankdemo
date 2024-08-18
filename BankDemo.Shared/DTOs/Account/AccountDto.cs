namespace BankDemo.Shared.DTOs.Account
{
    public enum AccountTypeDto
    {
        Base = 0,
        Default = 1,
        Deposit = 2
    }

    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public bool Locked { get; set; }
        public AccountTypeDto AccountType { get; set; }
        public string OwnerId { get; set; }
        public Guid CurrencyId { get; set; }
    }
}
