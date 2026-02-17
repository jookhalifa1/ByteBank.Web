using Domain.Contract;
using Domain.Entity;
using Domain.Entity.BankModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using persistenceLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace persistenceLayer
{
    public class DataSeeding : IDataSeeding
    {
        private readonly StoreDbContext context;

        public DataSeeding( StoreDbContext context)
        {
            this.context = context;
        }
        public async Task SeedingAsyn()
        {
           var  hasBank = context.Set<Bank>();
            var hasCardBank= context.Set<CardBank>();

            if (!hasBank .Any())
            {
                var filepath = "D:\\ProjectBackEnd\\ByteBank.Web\\presistenceLayer\\JsonFiles\\Bank.json";
                await seedjsonAsync<Bank, int>(filepath, hasBank);  
            await context.SaveChangesAsync();


            }

            if (!hasCardBank.Any())
            {
                var FilePath = "D:\\ProjectBackEnd\\ByteBank.Web\\presistenceLayer\\JsonFiles\\CardBank.json";
                await seedjsonAsync<CardBank, string>(FilePath, hasCardBank);
                await context.SaveChangesAsync();

            }
        }


        private async Task seedjsonAsync<T,Tkey>(string filePath,DbSet<T> entities   ) where T : BaseEntity<Tkey>
        {
            if (!File.Exists(filePath)) return;
            var data = File.OpenRead(filePath);
            var jsondata = JsonSerializer.Deserialize<List<T>>(data, new JsonSerializerOptions());
            if(jsondata is not  null)
                await entities.AddRangeAsync(jsondata);
            
        }

    }
}
