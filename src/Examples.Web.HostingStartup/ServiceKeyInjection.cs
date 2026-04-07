using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

[assembly: HostingStartup(typeof(Examples.Web.HostingStartup.ServiceKeyInjection))]

namespace Examples.Web.HostingStartup;

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
