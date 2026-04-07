using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

[assembly: HostingStartup(typeof(Examples.Web.HostingStartup.ConfigurationInjection))]

namespace Examples.Web.HostingStartup;

public class ConfigurationInjection : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        // This value takes precedence.
        builder.ConfigureAppConfiguration(config =>
        {
            var dict = new Dictionary<string, string?>
            {
                {"ConfigurationKey1",
                    "From IHostingStartup: Higher priority " +
                    "than the app's configuration."},
            };

            config.AddInMemoryCollection(dict);
        });

        // This value is overridden by the app's configuration.
        {
            var dict = new Dictionary<string, string?>
            {
                {"ConfigurationKey2",
                    "From IHostingStartup: Lower priority " +
                    "than the app's configuration."},
            };

            var builtConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(dict)
                .Build();

            builder.UseConfiguration(builtConfig);
        }
    }
}
