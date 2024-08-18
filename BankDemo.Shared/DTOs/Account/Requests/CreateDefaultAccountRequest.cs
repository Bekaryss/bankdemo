using System.ComponentModel.DataAnnotations;

namespace BankDemo.Shared.DTOs.Account.Requests
{
    public class CreateDefaultAccountRequest
    {
        [Required]
        public Guid CurrencyId { get; set; }

        [Required]
        public string AccountName { get; set; }
    }
}
