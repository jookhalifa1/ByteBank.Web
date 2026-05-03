using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.BankModule
{
     public class Transactions:BaseEntity< Guid>
    {
        public  string SenderCard { get; set; }
        [ForeignKey(nameof(SenderCard))]
        public CardBank SenderCardBank { get; set; }


        public  string ReciverCard { get; set; }

        [ForeignKey(nameof(ReciverCard))]
        public CardBank ReciverCardBank { get; set; }

    

        public DateTime DateOfTransaction { get; set; }= DateTime.Now;

        public decimal Fee { get; set; }

       public decimal Amount { get; set; }

        public decimal Total => Amount + Fee;

        public bool status { get; set; }=default(bool);

    }
}
