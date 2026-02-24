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

        public AuthenticationServices( UserManager< ApplicationUser> userManager,IMapper mapper,RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }
        public async Task<Result<UserDto>> loginAsync(LoginDto login)
        {


             var userEmail= await userManager.FindByEmailAsync(login.Email);
            if (userEmail is   null) return Result<UserDto>.Failure ( Error.Validation("Error While Validation"));
            var password = await userManager.CheckPasswordAsync(userEmail,login.Password);
            if (!password) return Result<UserDto>.Failure(Error.Validation("Error While Validation"));
           


            var user = new UserDto()
            {
                Email =  userEmail.Email,
                DisplayName = userEmail.UserName,
                Token = await  CreateTokenAsync(userEmail)

            };
            return Result<UserDto>.Ok(user);

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
