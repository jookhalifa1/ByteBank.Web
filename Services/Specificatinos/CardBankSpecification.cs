using Domain.Entity.BankModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specificatinos
{
     public class CardBankSpecification :BaseSpecifications<CardBank,  string>
    {
        public CardBankSpecification():base(null) 
        {
            AddIclude(x => x.bank);
        }

       
        public CardBankSpecification(string id, bool ByUserId) : base(ByUserId ? x => x.userid == id : x => x.Id == id)
        {
            AddIclude(x => x.bank);
        }



    }
}
