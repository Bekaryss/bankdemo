using BankDemo.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankDemo.Domain.Entities.Account
{
    public enum AccountType
    {
        General = 0,
        Default = 1,
        Deposit = 2
    }

    public class Account : BaseObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public Guid CurrencyId { get; set; }
        public bool Locked { get; set; }
        public string? LockBy { get; set; }
        public AccountType AccountType { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
    }
}
