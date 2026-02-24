using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.CardBankDto;
using Shared.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
     public class CardBankController:ApiController
    {
        private readonly ICardBankServices bankServices;

        public CardBankController( ICardBankServices bankServices)
        {
            this.bankServices = bankServices;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardDto>>> GetAll()
        {
            var result= await bankServices.GetAllAsync();
            return HandelRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CardDto>> GetById(string id)
        {
            var result=await bankServices.GetByIdAsync(id);
            return HandelRequest(result);   
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreatCardBank(CreatBankDto creatBank)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await bankServices.CreateCardAsync(id, creatBank);
            return HandelRequest(result);

        }

        [HttpGet("My-Cards")]
        
        public async Task<ActionResult< IEnumerable< CardDto>>> GetAllById()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await bankServices.GetAllById(userid);

            return HandelRequest(result);

        }
    }
}
