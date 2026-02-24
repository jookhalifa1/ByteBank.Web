using AutoMapper;
using Domain.Entity.BankModule;
using Shared;
using Shared.CardBankDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
     public class CardBankprofile:Profile
    {
        public CardBankprofile()
        {
            CreateMap<CardBank, CardDto>().ReverseMap();
            CreateMap<Bank, BankDto>().ReverseMap();
            CreateMap<CreatBankDto, CardBank>().ReverseMap();
            CreateMap<TransactionDto, Transactions>().ReverseMap();
            CreateMap<Transactions, ReturnResultTrans>().ReverseMap();
        }
    }
}
