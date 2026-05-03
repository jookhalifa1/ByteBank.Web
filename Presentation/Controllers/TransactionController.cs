using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesAbstraction;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Presentation.Controllers
{
    [Authorize]
     public class TransactionController:ApiController
    {
        private readonly ITransactionsServices services;
        private readonly ILogger<TransactionController> logger;

        public TransactionController(ITransactionsServices services ,ILogger<TransactionController> logger)
        {
            this.services = services;
            this.logger = logger;
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
            try
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                logger.LogInformation($"User id from Token {user}", user);

                var result = await services.GetAllByAdminAsync(user);

                return HandelRequest(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in  GetAllForAdmin");
                throw;
            }


        }
        [HttpGet("GetAllTransByCard/{cardid}")]

        public async Task<ActionResult<IEnumerable<ReturnResultTrans>>> GetAllTransCard(  string cardid)
        {
            var result = await services.GetAllByCard(cardid);
            return HandelRequest(result);

        }

    }
}
