using AutoMapper;
using Domain.Contract.Repositories;
using Domain.Entity.BankModule;
using Domain.Entity.IdentityModule;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Specificatinos;
using ServicesAbstraction;
using Shared.CardBankDto;
using Shared.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CardBankServices : ICardBankServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public CardBankServices( IUnitOfWork unitOfWork,IMapper mapper,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }

         

        public async Task<Result<ResultCreateCardDto>> CreateCardAsync( string  id,CreatBankDto creatBankDto)
        {
            var User = await userManager.FindByIdAsync(id);
            if (User == null)
                return Result<ResultCreateCardDto>.Failure(Error.NotFound("User not found"));
            
            

            var card  = mapper.Map<CardBank>(creatBankDto);
            var random=new Random();
            card .Id = string.Concat(Enumerable.Range(0, 16)
    .Select(_ => random.Next(0, 10).ToString()));

            card .userid = User.Id;



          await  unitOfWork.GetRepo<CardBank,  string>().AddAsync(card );
           var x=  await unitOfWork.SaveChangeRepoAsync();
            var bank = await unitOfWork
   .GetRepo<Bank, int>()
   .GetByIdAsync(card.BankId);

            var bankdto = mapper.Map<BankDto>(bank);
            if (x>0)
            {

                var result = new ResultCreateCardDto(
                    id: card.Id,
                    card.Name,
                    card.ExpireDate,
                    card.BankId,
                    card.Amount,
                    bankdto

                    );
                return Result<ResultCreateCardDto>.Ok(result);
            }
            else
                return Result<ResultCreateCardDto>.Failure( Error.UnAuthorized());

        }

        public async Task<Result<IEnumerable<CardDto>>> GetAllAsync()
        {
            var CardSpec = new CardBankSpecification();
            var result= await unitOfWork.GetRepo<CardBank, string>().GetAllSpecificationAsync(CardSpec);
                
            if (result is null) return Error.Failure();
            var carddto = mapper.Map<IEnumerable<CardDto>>(result);

            return Result<IEnumerable<CardDto>>.Ok( carddto);


        }

        public async Task<Result<IEnumerable<ResultCreateCardDto>>> GetAllById(string Userid)
        {
            var user = await userManager.FindByIdAsync(Userid);
            if(user is null)
            {
                return Error.UnAuthorized();
            }

            var cardspec = new CardBankSpecification(user.Id,true );
             
            var result = await unitOfWork.GetRepo<CardBank,   string>().GetAllSpecificationAsync(cardspec);
            var resultdto= mapper.Map<IEnumerable< ResultCreateCardDto>>(result);
            return Result<IEnumerable<ResultCreateCardDto>>.Ok( resultdto);
        }

        public async Task<Result<ResultCreateCardDto>> GetByIdAsync(  string id)
        {
            var cardspec = new CardBankSpecification(id,false );
           
            var result = await unitOfWork.GetRepo<CardBank, string>().GetByIdAsync(  cardspec);

            var r = mapper.Map<ResultCreateCardDto>(result);
             if(result is null ) return Error.Failure();
            return Result<ResultCreateCardDto>.Ok(r);

        }
    }
}
