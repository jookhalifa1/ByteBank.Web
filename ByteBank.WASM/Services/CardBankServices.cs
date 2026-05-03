using Shared.CardBankDto;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
namespace ByteBank.WASM.Services
{
    public class CardBankServices:BaseServices
    {
        private readonly HttpClient client;

        public CardBankServices(HttpClient client)
        {
            this.client = client;
        }
        //public async Task<IEnumerable<ResultCreateCardDto>> GetAllUser()
        //{
        //    var responce = await client.GetAsync("/api/CardBank/My-Cards");
        //    if (!responce.IsSuccessStatusCode)
        //    {
        //        var error = await responce.Content.ReadAsStringAsync();
        //        Console.WriteLine(error);


        //        throw new Exception("Error From Server  ");
        //    }
        //    else
        //    {
        //        var result = await responce.Content.ReadFromJsonAsync<IEnumerable<ResultCreateCardDto>>();


        //        if (result is null)
        //        {
        //            Console.WriteLine($"{0} in api ");



        //        }


        //            return result;

        //    }

        //}

        public async Task<ApiResult<IEnumerable<ResultCreateCardDto>>> GetAllUser()
        {
            var response = await client.GetAsync("/api/CardBank/My-Cards");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<IEnumerable<ResultCreateCardDto>>();

                return new ApiResult<IEnumerable<ResultCreateCardDto>>
                {
                    IsSuccess = true,
                    Data = data ?? Enumerable.Empty<ResultCreateCardDto>()
                };
            }

            return   await HandleError<IEnumerable<ResultCreateCardDto>>(response);
        }


        //public async Task<CardDto> GetById (string id)
        //{
        //    if(!string.IsNullOrEmpty(id))
        //    {
        //        var responce = await client.GetAsync($"/api/CardBank/{id}");
        //        if (!responce.IsSuccessStatusCode)
        //        {
        //            var error =await responce.Content.ReadAsStringAsync();
        //            Console.WriteLine($"Error From Api : {error}");

        //            throw new Exception("Card Not Found");
        //        }
        //        else
        //        {
        //            var data= await responce.Content.ReadFromJsonAsync<CardDto>();
        //            return data;

        //        }

        //    }
        //    else
        //    {
        //        throw new Exception("The card is not found ");
        //    }
        //}

        public async Task<ApiResult<ResultCreateCardDto>> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new ApiResult<ResultCreateCardDto>
                {
                    IsSuccess = false,
                    Message = "Invalid Id"
                };
            }

            var response = await client.GetAsync($"/api/CardBank/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResultCreateCardDto>();

                return new ApiResult<ResultCreateCardDto>
                {
                    IsSuccess = true,
                    Data = data
                };
            }

            return await HandleError<ResultCreateCardDto>(response);
        }



        //public async Task< ResultCreateCardDto> createBank(CreatBankDto card) {


        //    var responce = await client.PostAsJsonAsync<CreatBankDto>("/api/CardBank", card);

        //    if (!responce.IsSuccessStatusCode)
        //    {
        //       var error = await responce.Content.ReadAsStringAsync();

        //        Console.WriteLine($"Creat bank Error : {error}");

        //        throw new Exception("Faild Creat");


        //    }
        //    else
        //    {
        //        var data = await responce.Content.ReadFromJsonAsync<ResultCreateCardDto>();

        //        return data;
        //    }


        //}



        public async Task<ApiResult<ResultCreateCardDto>> createBank(CreatBankDto card)
        {
            var response = await client.PostAsJsonAsync("/api/CardBank", card);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResultCreateCardDto>();

                return new ApiResult<ResultCreateCardDto>
                {
                    IsSuccess = true,
                    Data = data
                };
            }

            return await HandleError<ResultCreateCardDto>(response);
        }


        public void SetToken(string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }



    }
 
