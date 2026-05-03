using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Shared;
using Shared.CardBankDto;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ByteBank.WASM.Services
{
    public class TransactionServicess :BaseServices
    {
        private readonly HttpClient client;
        private HubConnection hubConnection;

        public TransactionServicess(HttpClient client)
        {
            this.client = client;
        }

        // =====================
        // OLD DATA
        // =====================
        public async Task<List<ReturnResultTrans>> GetOldTransactions()
        {
            return await client.GetFromJsonAsync<List<ReturnResultTrans>>("/api/Transaction");

        }

        // =====================
        // CREATE TRANSACTION
        // =====================
        public async Task<ApiResult< ReturnResultTrans>> CreateTrans(TransactionDto transaction)
        {
            var response = await client.PostAsJsonAsync("/api/Transaction", transaction);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ReturnResultTrans>();

                return new ApiResult<ReturnResultTrans>
                {
                    IsSuccess = true,
                    Data = data
                };
            }

            return await HandleError<ReturnResultTrans>(response) ;
        }

        // =====================
        // SIGNALR START
        // =====================
        public async Task StartSignalR(string token, string bankId, Action<ReturnResultTrans> onNewTransaction)
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://byte-0235.tryasp.net/transactionHub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);

                })
                .WithAutomaticReconnect()
                .Build();

            hubConnection.On<ReturnResultTrans>("NewTransaction", trans =>
            {
                onNewTransaction?.Invoke(trans);
            });

            await hubConnection.StartAsync();

            await hubConnection.InvokeAsync("JoinGroup", bankId);
        }

        public async Task StopSignalR()
        {
            if (hubConnection != null)
                await hubConnection.StopAsync();
        }


        public async Task<ApiResult<IEnumerable<ReturnResultTrans>>> GetAllCard(  string cardid)
        {
            if (string.IsNullOrEmpty(cardid.ToString()))
            {
                return new ApiResult<IEnumerable<ReturnResultTrans>>
                {
                    IsSuccess = false,
                    Message = "Invalid Id"
                };
            }

            var response = await client.GetAsync($"/api/Transaction/GetAllTransByCard/{cardid}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<IEnumerable<ReturnResultTrans>>();

                return new ApiResult<IEnumerable<ReturnResultTrans>>
                {
                    IsSuccess = true,
                    Data = data
                };
            }

            return await HandleError<IEnumerable<ReturnResultTrans>>(response);

        }

        public void SetToken(string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
