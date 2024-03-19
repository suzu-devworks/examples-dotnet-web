using System.Reflection;

[assembly: HostingStartup(typeof(Examples.Web.WebUI.Infrastructure.HostingStartup))]

namespace Examples.Web.WebUI.Infrastructure;

public class HostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
         {
             var dict = new Dictionary<string, string?>
             {
                {"DevAccount", "DEV_5555555-5555"},
                {"ProdAccount", "PROD_6666666-6666"}
             };

             config.AddInMemoryCollection(dict);
         });
    }
}
