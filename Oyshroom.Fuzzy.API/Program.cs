using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Oyshroom.Fuzzy.API
{
    public class Program
    {

#if DEBUG
        public static void Main(string[] args)
        {
            try
            {
                //Log.Logger = new LoggerConfiguration()
                // .MinimumLevel.Debug()
                // .WriteTo.File("C:\\Oyshroom.API\\OyshroomAPI.log")
                // .CreateLogger();

                //Log.Information("Running Application");

                CreateHostBuilder(args).Build().Run();


            }
            catch (Exception ex)
            {
                throw ex;
                //Log.Error($"Error {ex.Message}");
                //Log.Error($"{ex.InnerException.Message}");
                //Log.Error($"{ex.StackTrace}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                  Host.CreateDefaultBuilder(args)
                      .ConfigureWebHostDefaults(webBuilder =>
                      {
                          webBuilder.UseStartup<Startup>();
                      });
#else
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Debug()
           .WriteTo.File("C:\\Oyshroom.Dashboard\\OyshroomDashboard.txt")
           .CreateLogger();

            try
            {
                var config = new ConfigurationBuilder()
               .SetBasePath(GetBasePath())
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

                var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls("http://*:3010") // Using * to bind to all network interfaces
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseConfiguration(config)
                    .UseStartup<Startup>()
                    .Build();

                Log.Information("Running Application");

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Error($"Error {ex.Message}");
                Log.Error($"{ex.InnerException.Message}");
                Log.Error($"{ex.StackTrace}");

            }

        }
#endif


        private static string GetBasePath()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;
            if (isDevelopment)
            {
                return Directory.GetCurrentDirectory();
            }
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }

    }
}
