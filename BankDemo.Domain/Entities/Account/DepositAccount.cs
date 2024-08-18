using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankDemo.Domain.Entities.Account
{
    public class DepositAccount : Account
    {
        public DateTime ExpireTime { get; set; }
        public decimal Percentage { get; set; }
    }
}
