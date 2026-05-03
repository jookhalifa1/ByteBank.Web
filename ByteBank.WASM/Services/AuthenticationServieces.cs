using ByteBank.WASM.Services;
using Shared.AuthenticationDto;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;

namespace ByteBank.WASM.Servies
{
    public class AuthenticationServieces: BaseServices
    {
        private readonly HttpClient httpClient;

        public AuthenticationServieces(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ApiResult< UserDto>> LoginServices(LoginDto login)
        {

            var x = await httpClient.PostAsJsonAsync<LoginDto>("/api/Authentication/login", login);
            if (x.IsSuccessStatusCode)
            {
                var data = await x.Content.ReadFromJsonAsync<UserDto>();

                return new ApiResult<UserDto>
                {
                    IsSuccess = true,
                    Data = data
                };
            }
            return await HandleError<UserDto>(x);

        }


        public async Task<  ApiResult< UserDto>> SignUp(RegisterDto registerDto)
        {
            var responce = await httpClient.PostAsJsonAsync<RegisterDto>("/api/Authentication/Register", registerDto);
            if ( responce.IsSuccessStatusCode)
            {
                 var data =await responce.Content.ReadFromJsonAsync<UserDto>();

                return new ApiResult<UserDto>
                {
                    IsSuccess = true,
                    Data = data
                };
            }
            return await HandleError<UserDto>(responce);


        }

        public async Task<UserDto> OtpVerfiy(OtpDto otp)
        {
            var responce = await httpClient.PostAsJsonAsync<OtpDto>("/api/Authentication/VerfiyOTP", otp);
            if (!responce.IsSuccessStatusCode)
            {
                var error =await responce.Content.ReadAsStringAsync();

                Console.WriteLine($"Otp API Error: {error}");


                throw new Exception("Invalid Code ");
            }
            var result = await responce.Content.ReadFromJsonAsync<UserDto>();
            
            return result;
        }
    }
}
