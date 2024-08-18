namespace BankDemo.Shared.DTOs.Account
{
    public class DepositAccountDto : AccountDto
    {
        public DateTime ExpireTime { get; set; }
        public decimal Percentage { get; set; }
    }
}
