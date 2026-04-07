[assembly: HostingStartup(typeof(Examples.Web.WebApp.Applications.Environments.MyHostingStartup))]

namespace Examples.Web.WebApp.Applications.Environments;

public class MyHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
            config.AddInMemoryCollection(new Dictionary<string, string?>
             {
                {"DevAccount", "DEV_5555555-5555"},
                {"ProdAccount", "PROD_6666666-6666"}
             }));
    }
}
