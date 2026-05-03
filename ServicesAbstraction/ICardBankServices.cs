using Shared.CardBankDto;
using Shared.ResultPattern;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
    public interface ICardBankServices
    {
        public Task<Result<IEnumerable<CardDto>>> GetAllAsync();

        public Task<Result<ResultCreateCardDto>> GetByIdAsync( string id);



        public Task<Result<ResultCreateCardDto>> CreateCardAsync( string id, CreatBankDto creatBankDto);

        public Task<Result<IEnumerable<ResultCreateCardDto>>> GetAllById(string id);


    }
}
