using System.ComponentModel.DataAnnotations;

namespace BankDemo.Shared.DTOs.Account.Requests
{
    public class TransferRequest
    {
        [Required]
        public Guid FromAccountId { get; set; }
        [Required]
        public Guid ToAccountId { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
