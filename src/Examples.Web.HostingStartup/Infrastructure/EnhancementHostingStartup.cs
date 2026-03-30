using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: HostingStartup(typeof(Examples.Web.Infrastructure.EnhancementHostingStartup))]

namespace Examples.Web.Infrastructure;

public class EnhancementHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            var dict = new Dictionary<string, string?>
            {
                {"DevAccount_FromLibrary", "DEV_1111111-1111"},
                {"ProdAccount_FromLibrary", "PROD_2222222-2222"},
            };

            config.AddInMemoryCollection(dict);
        });

        builder.ConfigureServices((context, services) =>
        {
            _ = context;

            var provider = services.BuildServiceProvider();
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<EnhancementHostingStartup>();

            logger.LogInformation("called Configure in {assembly}.",
                Assembly.GetExecutingAssembly().FullName);
        });

        // NOTIFICATION!
        // It seems that calling Configure(Action<IApplicationBuilder>) overwrites the application settings.

        // builder.Configure(app =>
        // {
        // });

        return;
    }
}
