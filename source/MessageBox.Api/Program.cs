using System;
using Autofac.Extensions.DependencyInjection;
using MessageBox.Core.Infrastructure;
using MessageBox.Core.Services.Logs;
using MessageBox.Data;
using MessageBox.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessageBox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogService>();
                    logger.LogErrorAsync(new CreateLogModel() { Title = "An error occurred seeding the DB.", Exception = ex });
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
       .UseServiceProviderFactory(new AutofacServiceProviderFactory())
       .ConfigureWebHostDefaults(webBuilder =>
       {
           webBuilder
               .UseStartup<Startup>()
               .ConfigureLogging(logging =>
               {
                   logging.ClearProviders();
                   logging.AddConsole();
               });
       });
    }
}
