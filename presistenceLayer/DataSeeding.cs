using Domain.Contract;
using Domain.Entity;
using Domain.Entity.BankModule;
using Domain.Entity.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using persistenceLayer.Data;
using System.Text.Json;

namespace persistenceLayer
{
    public class DataSeeding : IDataSeeding
    {
        private readonly StoreDbContext context;
        private readonly IdentityContext identityContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DataSeeding(StoreDbContext context, IdentityContext identityContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.identityContext = identityContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedingAsyn()
        {
            if (!await context.Set<Bank>().AnyAsync())
            {
                var path = Path.Combine(AppContext.BaseDirectory, "JsonFiles", "Bank.json");
                await seedjsonAsync<Bank, int>(path, context.Set<Bank>());
                await context.SaveChangesAsync();
            }

            if (!await context.Set<CardBank>().AnyAsync())
            {
                var path = Path.Combine(AppContext.BaseDirectory, "JsonFiles", "CardBank.json");
                await seedjsonAsync<CardBank,   string>(path, context.Set<CardBank>());
                await context.SaveChangesAsync();
            }
        }

        public async Task SeedingIdentityAsyn()
        {
            // 1. Create Role
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // 2. Prevent duplicate seeding
            if (await userManager.Users.AnyAsync()) return;

            // 3. Users
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Email = "jooSayed@gmail.com",
                    UserName = "JooSayed",
                    bankid = 1,
                    PhoneNumber = "01063078653",
                    address = new Address
                    {
                        city = "Cairo",
                        Country = "Helwan",
                        street = "51"
                    }
                },
                new ApplicationUser
                {
                    Email = "jooKhalifa@gmail.com",
                    UserName = "JooKhalifa",
                    bankid = 2,
                    PhoneNumber = "01063078654",
                    address = new Address
                    {
                        city = "Cairo",
                        Country = "Maddi",
                        street = "51"
                    }
                },
                new ApplicationUser
                {
                    Email = "Sayed@gmail.com",
                    UserName = "Sayed",
                    bankid = 3,
                    PhoneNumber = "01063078655",
                    address = new Address
                    {
                        city = "Cairo",
                        Country = "Montasr",
                        street = "51"
                    }
                }
            };

            // 4. Create + Add Role
            foreach (var user in users)
            {
                var result = await userManager.CreateAsync(user, "P@ssw0rd");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        private async Task seedjsonAsync<T, Tkey>(string filePath, DbSet<T> entities) where T : BaseEntity<Tkey>
        {
            if (!File.Exists(filePath)) return;

            if (await entities.AnyAsync()) return;

            var json = await File.ReadAllTextAsync(filePath);
            var data = JsonSerializer.Deserialize<List<T>>(json);

            if (data != null)
            {
                await entities.AddRangeAsync(data);
            }
        }
    }
}