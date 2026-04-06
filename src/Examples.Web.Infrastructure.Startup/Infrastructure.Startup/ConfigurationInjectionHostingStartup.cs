using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

[assembly: HostingStartup(typeof(Examples.Web.Infrastructure.Startup.ConfigurationInjectionHostingStartup))]

namespace Examples.Web.Infrastructure.Startup;

public class ConfigurationInjectionHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            var dict = new Dictionary<string, string?>
            {
                {
                    "ConfigurationKey1",
                    "From IHostingStartup: Higher priority than the app's configuration."
                },
            };

            config.AddInMemoryCollection(dict);
        });

        {
            var dict = new Dictionary<string, string?>
            {
                {
                    "ConfigurationKey2",
                    "From IHostingStartup: Lower priority than the app's configuration."
                },
            };

            var builtConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(dict)
                .Build();

            builder.UseConfiguration(builtConfig);
        }

        return;
    }
}
