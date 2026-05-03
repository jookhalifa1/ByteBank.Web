using Domain.Entity.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.BankModule
{
     public class CardBank:BaseEntity<string>
    {
        public string Name { get; set; } = default!;
        public DateTime ExpireDate { get; set; }=DateTime.Now.AddYears(5);

        public Bank  bank { get; set; }
        public int BankId { get; set; }

        public decimal Amount { get; set; }
     
        public  string userid { get; set; }


    }
}
