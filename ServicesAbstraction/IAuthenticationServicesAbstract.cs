using Shared.AuthenticationDto;
using Shared.ResultPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
     public interface IAuthenticationServicesAbstract
    {


        public Task<Result<UserDto>> loginAsync(LoginDto login);
        public Task<Result<UserDto>>  RegisterAsync(RegisterDto login);
        public Task<Result<UserDto>> VerifyOtpAsync( string email , string code);

    }
}
