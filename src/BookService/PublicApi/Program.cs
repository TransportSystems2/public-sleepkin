using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Pillow.Infrastructure.Data;
using Pillow.Infrastructure.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Serilog;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Fluentd;

namespace Pillow.PublicApi
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

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog((context, configuration) =>
                    {
                        configuration.Enrich.FromLogContext();
                        configuration.Enrich.WithMachineName();
                        configuration.ReadFrom.Configuration(context.Configuration);
                        configuration.WriteTo.Console(new ElasticsearchJsonFormatter());
                        configuration.WriteTo.Fluentd(
                            new FluentdSinkOptions(
                                    "fluent-bit", 
                                    24224, 
                                    context.HostingEnvironment.ApplicationName)
                            {LingerEnabled = false });
                    });
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task MigrateDB(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var bookContext = services.GetRequiredService<BookContext>();

                await TryConnectToDb(bookContext.Database);

                await bookContext.Database.MigrateAsync();
                await BookContextSeed.SeedAsync(bookContext, loggerFactory);

                var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
                await identityDbContext.Database.MigrateAsync();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await AppIdentityDbContextSeed.SeedAsync(userManager, roleManager);
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
