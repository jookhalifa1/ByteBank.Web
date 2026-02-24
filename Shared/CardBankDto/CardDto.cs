using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CardBankDto
{
     public class CardDto
    {
        public string Name { get; set; } = default!;
        public DateTime ExpireDate { get; set; }  

        public BankDto bank { get; set; }
        public int BankId { get; set; }

        public decimal Amount { get; set; }

        public string userid { get; set; }

    }
}
