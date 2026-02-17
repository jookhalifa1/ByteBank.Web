using Domain.Contract;

namespace ByteBank.web.Extenstions
{
    public static class Web
    {
        public static async Task<WebApplication> DataSeedinAsync( this WebApplication app)
        {
            var scop = app.Services.CreateAsyncScope();
            var dataseed = scop.ServiceProvider.GetRequiredService<IDataSeeding>();
             await dataseed.SeedingAsyn();
            return app;
        }

    }
}
