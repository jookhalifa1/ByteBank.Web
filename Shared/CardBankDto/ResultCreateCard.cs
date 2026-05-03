using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CardBankDto
{
     public class ResultCreateCardDto
    {
        public ResultCreateCardDto( string id, string name, DateTime expireDate, int bankId, decimal amount, BankDto Bank)
        {
            Id = id;
            Name = name;
            ExpireDate = expireDate;
            BankId = bankId;
            Amount = amount;
            bank = Bank;

        }

        public  string Id { get; set; }  
        public string Name { get; set; } = default!;
        public DateTime ExpireDate { get; set; }
        public BankDto bank { get; set; }


         
        public int BankId { get; set; }

        public decimal Amount { get; set; }

    }
}
