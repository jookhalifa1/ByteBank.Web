using AutoMapper;
using Domain.Entity.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServicesAbstraction;
using Shared.AuthenticationDto;
using Shared.ResultPattern;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationServices : IAuthenticationServicesAbstract
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IEmailServices emailServices;

        public AuthenticationServices( UserManager< ApplicationUser> userManager,IMapper mapper,RoleManager<IdentityRole> roleManager,IConfiguration configuration,IEmailServices emailServices)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.emailServices = emailServices;
        }
        public async Task<Result<UserDto>> loginAsync(LoginDto login)
        {


             var user= await userManager.FindByEmailAsync(login.Email);
            if (user is   null) return Result<UserDto>.Failure ( Error.Validation("Error While Validation"));
            var password = await userManager.CheckPasswordAsync(user,login.Password);
            if (!password) return Result<UserDto>.Failure(Error.Validation("Error While Validation"));



            //var userdto = new UserDto()
            //{
            //    Email =   user .Email,
            //    DisplayName = user.UserName,
            //    Token = await CreateTokenAsync(user)

            //};
            //return Result<UserDto>.Ok(userdto);

            var token = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
            await emailServices.SendEmailAsync(
                user.Email,
                "OTP Verification (ByteBank.Web)",
                $"Your login  OTP From ByteBank.Web is {token}"
                );
            return Result<UserDto>.Ok(new UserDto
            {
                Email = user.Email,
                DisplayName = user.UserName,
                Token="Please Go to VerfiyOTp To Recive Your Token :)"
            });

        }



        public async Task<Result<UserDto>> VerifyOtpAsync(string email, string code)
        {
            var user =await userManager.FindByEmailAsync (email);
            if (user is null)
                return   Result<UserDto>.Failure(Error.NotFound());
            var Isvalid = await userManager.VerifyTwoFactorTokenAsync(user, "Email", code);
            if (!Isvalid)
            {
                return Result<UserDto>.Failure(Error.NotFound());   
            }
            return new UserDto()
            {
                Email = user.Email,
                DisplayName = user.UserName,
                Token = await CreateTokenAsync(user)
            };

        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto register)
        {

        

            var addrees = mapper.Map<AddressDto, Address>(register.address);
            var user = new ApplicationUser()
            {
                Email = register.Email,
                UserName = register.Name,
                PhoneNumber = register.Phone,
                address = addrees,


            };

            var UserPassword = await userManager.CreateAsync(user, register.Password);
            if (UserPassword.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
                var userr = new UserDto()
                {
                    Email = register.Email,
                    Token = await CreateTokenAsync(user),
                    DisplayName = register.Name
                };
                return Result<UserDto>.Ok(userr);
            }
            var x = UserPassword.Errors.Select(x => Error.Validation(x.Code, x.Description)).ToList();
            return Result<UserDto>.Failure(x);

        }

       

        private async Task<string> CreateTokenAsync( ApplicationUser user)
        {
            var secertKey = configuration["JWTToken:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secertKey));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);



            var claim = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.NameId,user.Id),
            };
            var role = await userManager.GetRolesAsync(user);
            foreach (var item in role)
            {
                claim.Add(new Claim(ClaimTypes.Role, item));
            }
            var token = new JwtSecurityToken(
                issuer: configuration["JWTToken:Issuer"],
                audience: configuration["JWTToken:Audience"],
                signingCredentials: cred,
                claims: claim,
                expires: DateTime.UtcNow.AddHours(1)


                );

            var result = new JwtSecurityTokenHandler().WriteToken(token);
            
            return result;
              
                  
              



        }
    }
}
