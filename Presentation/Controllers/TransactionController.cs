using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
     public class TransactionController:ApiController
    {
        private readonly ITransactionsServices services;

        public TransactionController(ITransactionsServices services)
        {
            this.services = services;
        }

        [HttpPost]
        
        public async Task<ActionResult<ReturnResultTrans>> CreatTransaction(TransactionDto transaction)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await services.CreatTransAsync(id, transaction);
            return HandelRequest(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnResultTrans>>> GetAllForAdmin()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await services.GetAllByAdminAsync(user);

            return HandelRequest(result);


        }

    }
}
