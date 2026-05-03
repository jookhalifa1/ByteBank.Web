using ByteBank.web.Extenstions;
using Domain.Contract;
using Domain.Contract.Repositories;
using Domain.Entity.BankModule;
using Domain.Entity.IdentityModule;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using persistenceLayer;
using persistenceLayer.Data;
using persistenceLayer.Repos;
using Services;
using Services.MappingProfile;
using ServicesAbstraction;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace ByteBank.web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ 1. إعداد Serilog (قبل أي حاجة)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // ✅ 2. ربطه بالـ Host
            builder.Host.UseSerilog();


            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<IdentityContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddDbContext<StoreDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAutoMapper(c => c.AddProfile<AddressProfile>());
            builder.Services.AddAutoMapper(x => x.AddProfile<CardBankprofile>());

            builder.Services.AddScoped<IAuthenticationServicesAbstract, AuthenticationServices>();
            builder.Services.AddScoped<ICardBankServices, CardBankServices>();
            builder.Services.AddScoped<ITransactionsServices, TransactionsServices>();
            builder.Services.AddScoped<IEmailServices, EmailServices>();

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {


                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/transactionHub"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                }; 
                opt.SaveToken = true;

                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWTToken:Issuer"],
                    ValidAudience = builder.Configuration["JWTToken:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWTToken:SecretKey"]))
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                    policy.WithOrigins("https://byteblazor-0235.runasp.net", "https://localhost:7161")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
                           
            });

            builder.Services.AddSignalR();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            var app = builder.Build();

            app.UseStaticFiles();



            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "🔥 Unhandled Exception");
                    throw;
                }
            });

            if (!app.Environment.IsDevelopment())
            {
                await app.DataSeedinAsync();
                await app.DataSeedinIdentityAsync();
            }
            // Configure the HTTP request pipeline.
            //   if (app.Environment.IsDevelopment())
             
                app.UseSwagger();
                app.UseSwaggerUI();
                // }

                app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();

            app.UseCors("AllowAll");

                app.UseAuthentication();   // 👈 الوحيد اللي اتضاف

                app.UseAuthorization();

                app.MapControllers();
                app.MapHub<TransactionHub>("/transactionHub");

                app.Run();
             
        }
    }
}