using Blazored.SessionStorage;
using ByteBank.WASM.Services;
using ByteBank.WASM.Servies;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ByteBank.WASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddScoped<AuthenticationServieces>();
            builder.Services.AddScoped<CardBankServices>();
            builder.Services.AddScoped<TransactionServicess>();

            builder.Services.AddBlazoredSessionStorage();

           builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://byte-0235.tryasp.net") });
          //  builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7175") });



            await builder.Build().RunAsync();
        }
    }
}
