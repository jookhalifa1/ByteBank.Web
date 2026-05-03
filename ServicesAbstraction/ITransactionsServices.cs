using Shared;
using Shared.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
     public interface ITransactionsServices
    {
        public Task<Result<ReturnResultTrans>> CreatTransAsync(string id ,TransactionDto transaction);


        public Task<Result<IEnumerable< ReturnResultTrans>>> GetAllByAdminAsync(  string Adminid);


        public Task<Result<IEnumerable<ReturnResultTrans>>> GetAllByCard( string id);
    }
}
