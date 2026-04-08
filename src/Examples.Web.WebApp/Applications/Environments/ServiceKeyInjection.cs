[assembly: HostingStartup(typeof(Examples.Web.WebApp.Applications.Environments.ServiceKeyInjection))]

namespace Examples.Web.WebApp.Applications.Environments;

/// <summary>
/// A sample <see cref="IHostingStartup"/> implementation to demonstrate the hosting startup assembly discovery and configuration ordering for service key injection.
/// </summary>
public class ServiceKeyInjection : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(static config =>
        {
            var dict = new Dictionary<string, string?>
            {
                    {"DevAccount_FromLibrary", "DEV_1111111-1111"},
                    {"ProdAccount_FromLibrary", "PROD_2222222-2222"}
            };

            config.AddInMemoryCollection(dict);
        });
    }
}
