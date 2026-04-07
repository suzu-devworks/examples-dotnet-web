using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: HostingStartup(typeof(Examples.Web.HostingStartup.EnhancementHostingStartup))]

namespace Examples.Web.HostingStartup;

public class EnhancementHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Enhancement:ConfigureAppConfiguration",
                    "called ConfigureAppConfiguration." }
            });
        });

        builder.ConfigureServices((context, services) =>
        {
            var provider = services.BuildServiceProvider();
            var logger = provider.GetRequiredService<ILogger<EnhancementHostingStartup>>();
            logger.LogWarning("called ConfigureServices.");
        });

        // NOTIFICATION!
        // It seems that calling Configure(Action<IApplicationBuilder>) overwrites the application settings.

        // builder.Configure(app =>
        // {
        // });
    }
}
