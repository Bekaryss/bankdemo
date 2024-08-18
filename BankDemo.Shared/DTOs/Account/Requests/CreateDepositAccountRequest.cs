using System.ComponentModel.DataAnnotations;

namespace BankDemo.Shared.DTOs.Account.Requests
{
    public class CreateDepositAccountRequest
    {

        [Required]
        public Guid CurrencyId { get; set; }

        [Required]
        public string AccountName { get; set; }

        [Required]
        public int DepositLifeTimeInMounth { get; set; }

        [Required]
        public decimal DepositPercentage { get; set; }
    }
}
