using AutoMapper;
using Domain.Contract.Repositories;
using Domain.Entity.BankModule;
using Domain.Entity.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Services.Specificatinos;
using ServicesAbstraction;
using Shared;
using Shared.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Services
{
    public class TransactionsServices : ITransactionsServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICardBankServices cardBankServices;
        private readonly IHubContext<TransactionHub> hubContext;

        public TransactionsServices( UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork,IMapper mapper,ICardBankServices cardBankServices,IHubContext<TransactionHub> hubContext)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cardBankServices = cardBankServices;
            this.hubContext = hubContext;
        }



        public async Task<Result<ReturnResultTrans>> CreatTransAsync(string id, TransactionDto transaction)
                {

                    var repo = unitOfWork.GetRepo<Transactions,  Guid>();
                    var user = await userManager.FindByIdAsync(id);
                    if (user == null)
                        return Error.NotFound($"{id} is not found ");


                    var cardbanksender = await  unitOfWork.GetRepo<CardBank,   string>().GetByIdAsync(transaction.SenderCard);
            if (cardbanksender is null) return Error.NotFound($"{transaction.SenderCard} is not found ");

                    if (user.Id != cardbanksender.userid) return Error.Forbidden($"you are not allowed to access this card {transaction.SenderCard} ");

                    var cardbankresiver = await unitOfWork.GetRepo<CardBank,  string>().GetByIdAsync(transaction.ReciverCard) ;
                    if (cardbankresiver is null) return Error.Validation("Receiver card does not exist. Please check the card number.");
            if (transaction.SenderCard == transaction.ReciverCard)
                        return Error.Validation("You can't transfer to the same card");


            
                    decimal fee = 0;
                    if (cardbanksender.BankId != cardbankresiver.BankId)
                    {
                      fee = transaction.Amount * 0.05m;
               
                    }
                    if (transaction.Amount+fee > cardbanksender.Amount) return Error.Failure($"your balance is not enough to do this transaction your balance is {cardbanksender.Amount}  ");

            
                        
               
                       cardbanksender.Amount-=transaction.Amount;
                        cardbanksender.Amount-=fee;
                        
            unitOfWork.GetRepo<CardBank,   string>().Update(cardbanksender);
           

                       
                        cardbankresiver.Amount+=transaction.Amount;
             
            unitOfWork.GetRepo<CardBank,  string >().Update(cardbankresiver);


                        var trans=mapper.Map<Transactions>(transaction);
            trans.Id=Guid.NewGuid(); 
            trans.Fee=fee;
            trans.status=true;

            var result=mapper.Map <ReturnResultTrans>(trans);
                        await repo.AddAsync(trans);
                    var x= await unitOfWork.SaveChangeRepoAsync();





            if (x > 0)
            {

                await hubContext.Clients
     .Group(trans.SenderCardBank.BankId.ToString())
     .SendAsync("NewTransaction", trans);

                return Result<ReturnResultTrans>.Ok(result);
            }
            else
                return Error.Failure("Failed to create transaction");









                }

        public async Task<Result<IEnumerable< ReturnResultTrans>>> GetAllByAdminAsync(  string Adminid)
        {
            var user = await  userManager.FindByIdAsync(Adminid.ToString());

            if (user is null) return Error.NotFound();

            var Roles  = await userManager.GetRolesAsync(user);

            if (Roles.Contains("Admin"))
            {
                var transspec = new TransSpecification(true, user.bankid);

                var x = await unitOfWork.GetRepo<Transactions,  Guid>().GetAllSpecificationAsync(transspec);
                if (x is null) return Error.NotFound();
                var result = mapper.Map<IEnumerable<ReturnResultTrans>>(x);

                return Result<IEnumerable<ReturnResultTrans>>.Ok( result);
            }
            return Error.UnAuthorized();
             
        }

        public async Task<Result<IEnumerable<ReturnResultTrans>>> GetAllByCard(  string id)
        {
            if (string.IsNullOrWhiteSpace(id.ToString())) return Error.NotFound($"{id} is not found  ");

            var data =  await cardBankServices.GetByIdAsync(id);
            if (data is null || !data.IsSuccess)
                return Error.NotFound($"Card {id} not found");


            var spec = new TransSpecification(data.Value.Id);
            var trans = await unitOfWork.GetRepo<Transactions,  Guid>().GetAllSpecificationAsync(spec);
            if (trans == null || !trans.Any())
                return Result<IEnumerable<ReturnResultTrans>>.Ok(new List<ReturnResultTrans>());
            var x = mapper.Map<IEnumerable<ReturnResultTrans>>(trans);

            return Result<IEnumerable<ReturnResultTrans>>.Ok(x);

        }
    }
}
