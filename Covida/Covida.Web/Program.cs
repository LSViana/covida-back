using System;
using System.Threading.Tasks;
using Covida.Data.Postgre;
using Covida.Data.Scaffold;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Covida.Web
{
    public static class Program
    {
        private static bool seedHandled = false;
        private static IWebHost webHost;
        private const string seedCommand = "seed";

        public static void Main(string[] args)
        {
            webHost = CreateWebHostBuilder(args)
                .Build();
            HandleArguments(args);
            // Initialize app
            webHost.Run();
        }

        private static void HandleArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                // Offset is used to "consume" next arguments from the current one
                var offset = HandleSeed(args, i).Result;
                i += offset;
            }
        }

        private static async Task<int> HandleSeed(string[] args, int i)
        {
            if (seedHandled)
            {
                return 0;
            }
            seedHandled = true;
            //
            var currentArg = args[i];
            if (currentArg.Equals(seedCommand, StringComparison.CurrentCultureIgnoreCase))
            {
                var scope = webHost.Services.CreateScope();
                var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder<CovidaDbContext>>();
                await seeder.Run();
                await seeder.Commit();
            }
            return 0;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureKestrel((context, options) =>
                {
                    options.AddServerHeader = false;
                });
    }
}
