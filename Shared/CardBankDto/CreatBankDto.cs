using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CardBankDto
{
     public class CreatBankDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(4    )]
        public string Name { get; set; } = default!;
        public DateTime ExpireDate { get; set; } = DateTime.Now.AddYears(2);

        [Range(1,3)]
        public int BankId { get; set; }
        [Range(1000,50000)]
        public decimal Amount { get; set; }

        
    }
}
