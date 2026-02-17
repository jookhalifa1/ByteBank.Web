
using ByteBank.web.Extenstions;
using Domain.Contract;
using Domain.Contract.Repositories;
using Domain.Entity.IdentityModule;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using persistenceLayer;
using persistenceLayer.Data;
using persistenceLayer.Repos;
using Services;
using Services.MappingProfile;
using ServicesAbstraction;
using System.Threading.Tasks;

namespace ByteBank.web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<IdentityContext>(option => {
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));


                 });
            builder.Services.AddDbContext<StoreDbContext>(option => {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            builder.Services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<IdentityContext>();
            builder.Services.AddAutoMapper(c=>c.AddProfile<AddressProfile>());
            builder.Services.AddScoped<IAuthenticationServicesAbstract,  AuthenticationServices>();

            var app = builder.Build();
            await app.DataSeedinAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
