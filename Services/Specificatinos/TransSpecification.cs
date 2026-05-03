using Domain.Entity.BankModule;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specificatinos
{
     public class TransSpecification:BaseSpecifications<Transactions, Guid>
    {
        public TransSpecification():base(null)
        {
            AddIclude(x => x.SenderCardBank);
            AddIclude(x => x.ReciverCardBank);


        }
        public TransSpecification(bool adminid, int  id ) : base(adminid ? (Expression<Func<Transactions,bool>>) (x=>x.SenderCardBank.BankId==id ||  x.ReciverCardBank.BankId==id) : null  )
        {
            AddIclude(x => x.SenderCardBank);
            AddIclude(x => x.ReciverCardBank);


        }

        public TransSpecification(  string cardid) : base(x => x.SenderCard == cardid)
        {
            AddIclude(x => x.SenderCardBank);
            AddIclude(x => x.ReciverCardBank);

        }
    }
}
