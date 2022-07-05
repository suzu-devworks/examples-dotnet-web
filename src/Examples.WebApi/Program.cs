using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Examples.WebApi.Infrastructure.Startup;

namespace Examples.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder
                    => webBuilder
                        .ConfigureKestrel(serverOptions => serverOptions.AddServerHeader = false)
                        .UseStartup<Startup>())
                .ConfigureLogging(logging
                    => logging.UseCustomLogging());

    }

}
