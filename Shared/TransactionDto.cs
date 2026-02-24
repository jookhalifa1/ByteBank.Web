using Shared.CardBankDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
     public class TransactionDto
    {
        public string SenderCard { get; set; }
        
         


        public string ReciverCard { get; set; }

         


       

       

        public decimal Amount { get; set; }

       

        
    }
}
