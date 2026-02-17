using Domain.Entity.IdentityModule;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Domain.Entity.BankModule
{
    public class Bank:BaseEntity<int>
    {
        public string Name { get; set; } = default!;

         
        public string userid { get; set; }

    }
}