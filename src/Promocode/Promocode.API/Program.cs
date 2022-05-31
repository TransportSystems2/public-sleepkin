using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PromoCode.API.Infrastructure;

namespace PromoCode.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build();

            await MigrateDB(host);
            
            await host.RunAsync();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        
        private static async Task MigrateDB(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var promocodeContext = services.GetRequiredService<PromoCodeContext>();

                await TryConnectToDb(promocodeContext.Database);

                await promocodeContext.Database.MigrateAsync();
                await PromoCodeContextSeed.SeedAsync(promocodeContext);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

        private static async Task TryConnectToDb(DatabaseFacade db, 
            int maxRetryCount = 5,
            int maxRetryDelayInSeconds = 5)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            var currentRetry = 0;
            while (!await db.CanConnectAsync() && currentRetry < maxRetryCount)
            {
                await Task.Delay(TimeSpan.FromSeconds(maxRetryDelayInSeconds));
                currentRetry++;
            }
        }
    }
}