using Shared.CardBankDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
     public class TransactionDto
    {
        [Required]
        public   string SenderCard { get; set; }



        [Required]
        public  string ReciverCard { get; set; }







        [Range(100,50000)]
        public decimal Amount { get; set; }

       

        
    }
}
