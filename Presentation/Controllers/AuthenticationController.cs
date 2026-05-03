using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.AuthenticationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
     
     public class AuthenticationController: ApiController
    {
        private readonly IAuthenticationServicesAbstract authenticationServices;

        public AuthenticationController(IAuthenticationServicesAbstract authenticationServices)
        {
            this.authenticationServices = authenticationServices;
        }

        [HttpPost("Login")]

         public async Task<ActionResult<UserDto>> Login( LoginDto login)
        {
            var result = await authenticationServices.loginAsync(login);

            return HandelRequest(result);
        }
        //[HttpPost("VerfiyOTP")]

        //public async Task<ActionResult<UserDto>> VerfiyOTP( OtpDto otp)
        //{
           
            
        //    var result = await authenticationServices.VerifyOtpAsync( otp.Email , otp.Code);
        //    return HandelRequest(result);
        //}



        [HttpPost("Register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            var result = await authenticationServices.RegisterAsync(register);

            return HandelRequest(result);
        }



    }
}
