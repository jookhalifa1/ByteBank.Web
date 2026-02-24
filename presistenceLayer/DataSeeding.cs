using Domain.Contract;
using Domain.Entity;
using Domain.Entity.BankModule;
using Domain.Entity.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client;
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
        private readonly IdentityContext identityContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DataSeeding( StoreDbContext context ,IdentityContext identityContext,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.identityContext = identityContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
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
        public async Task SeedingIdentityAsyn()
        {
            var adminuser = await userManager.GetUsersInRoleAsync("Admin");
            if (adminuser.Count==0)
            {
                var addressx = new Address()
                {
                    city = "Cairo",
                    Country = "Helwan",
                    street = "51"
                };
                var adminusermanager = new ApplicationUser()
                {
                    Email = "jooSayed@gmail.com",
                    bankid = 1,
                    UserName = "JooSayed",
                    address = addressx,
                    PhoneNumber = "01063078653"

                };
                var password = await userManager.CreateAsync(adminusermanager, "P@ssw0rd");
                var address01 = new Address()
                {
                    city = "Cairo",
                    Country = "Maddi",
                    street = "51"
                };
                var address02 = new Address()
                {
                    city = "Cairo",
                    Country = "Montasr",
                    street = "51"
                };
                var adminusermanager01 = new ApplicationUser()
                {
                    Email = "jooKhalifa@gmail.com",
                    bankid = 2,
                    UserName = "JooKhalifa",
                    address = address01,
                    PhoneNumber = "01063078654"

                };
                var password01 = await userManager.CreateAsync(adminusermanager01, "P@ssw0rd");
                var adminusermanager02 = new ApplicationUser()
                {
                    Email = "Sayed@gmail.com",
                    bankid = 3,
                   UserName = "Sayed",
                    address = address02,
                    PhoneNumber = "01063078654"

                };
            
                var password02 = await userManager.CreateAsync(adminusermanager02, "P@ssw0rd");


                if (password.Succeeded)
                {
                   await  userManager.AddToRoleAsync(adminusermanager, "Admin");

                    await identityContext.SaveChangesAsync();

                }
                if (password01.Succeeded)
                {
                   await userManager.AddToRoleAsync(adminusermanager01, "Admin");
                    await identityContext.SaveChangesAsync();

                }
                if (password02.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminusermanager02, "Admin");
                    await identityContext.SaveChangesAsync();

                }
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
