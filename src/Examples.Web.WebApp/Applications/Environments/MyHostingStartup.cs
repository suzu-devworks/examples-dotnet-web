[assembly: HostingStartup(typeof(Examples.Web.WebApp.Applications.Environments.MyHostingStartup))]

namespace Examples.Web.WebApp.Applications.Environments;

public class MyHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Demo:Source:MyHostingStartup:ConfigurePattern"] = "Defined MyHostingStartup",
                ["Demo:OrderProbe:All"] = "MyHostingStartup ConfigureAppConfiguration",
                ["Demo:OrderProbe:ConfigurePattern"] = "MyHostingStartup ConfigureAppConfiguration",
                ["Demo:Compare:MyHostingStartup"] = "From MyHostingStartup ConfigureAppConfiguration",
                ["Demo:Compare:ConfigureVsAppSettings"] = "From MyHostingStartup ConfigureAppConfiguration",
                ["Demo:Compare:ConfigureVsProgramConfigManager"] = "From MyHostingStartup ConfigureAppConfiguration",
            }));

        var builtConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Demo:Source:MyHostingStartup:UsePattern"] = "Defined MyHostingStartup",
                ["Demo:OrderProbe:All"] = "MyHostingStartup ConfigureAppConfiguration",
                ["Demo:OrderProbe:UsePattern"] = "MyHostingStartup UseConfiguration",
                ["Demo:Compare:MyHostingStartup"] = "From MyHostingStartup UseConfiguration",
                ["Demo:Compare:UseVsAppSettings"] = "From MyHostingStartup UseConfiguration",
                ["Demo:Compare:UseVsProgramConfigManager"] = "From MyHostingStartup UseConfiguration",
            })
            .Build();

        builder.UseConfiguration(builtConfig);
    }
}
