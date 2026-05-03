using Shared.CardBankDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
     public class ReturnResultTrans
    {
        public string SenderCard { get; set; }
     
        public ResultCreateCardDto SenderCardBank { get; set; }


        public string ReciverCard { get; set; }

        public ResultCreateCardDto ReciverCardBank { get; set; }




        public DateTime DateOfTransaction { get; set; } = DateTime.Now;

        public decimal Fee { get; set; }

        public decimal Amount { get; set; }

        public decimal Total => Amount + Fee;

        public bool status { get; set; } = default(bool);
    }
}
