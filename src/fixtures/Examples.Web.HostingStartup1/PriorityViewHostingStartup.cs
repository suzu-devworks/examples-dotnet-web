using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

[assembly: HostingStartup(typeof(Examples.Web.HostingStartup1.PriorityViewHostingStartup))]

namespace Examples.Web.HostingStartup1;

public class PriorityViewHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Demo:Source:HostingStartup1:ConfigurePattern"] = "Defined in HostingStartup1",
                ["Demo:OrderProbe:All"] = "HostingStartup1 ConfigureAppConfiguration",
                ["Demo:OrderProbe:ConfigurePattern"] = "HostingStartup1 ConfigureAppConfiguration",
                ["Demo:Compare:HostingStartup1"] = "From HostingStartup1 ConfigureAppConfiguration",
                ["Demo:Compare:ConfigureVsAppSettings"] = "From HostingStartup1 ConfigureAppConfiguration",
                ["Demo:Compare:ConfigureVsProgramConfigManager"] = "From HostingStartup1 ConfigureAppConfiguration",
                ["Demo:Compare:AssemblyOrder:ConfigureAppConfiguration"] = "From HostingStartup1 ConfigureAppConfiguration",
            }));

        var builtConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Demo:Source:HostingStartup1:UsePattern"] = "Defined in HostingStartup1",
                ["Demo:OrderProbe:All"] = "HostingStartup1 UseConfiguration",
                ["Demo:OrderProbe:UsePattern"] = "HostingStartup1 UseConfiguration",
                ["Demo:Compare:HostingStartup1"] = "From HostingStartup1 UseConfiguration",
                ["Demo:Compare:UseVsAppSettings"] = "From HostingStartup1 UseConfiguration",
                ["Demo:Compare:UseVsProgramConfigManager"] = "From HostingStartup1 UseConfiguration",
                ["Demo:Compare:AssemblyOrder:UseConfiguration"] = "From HostingStartup1 UseConfiguration",
            })
            .Build();

        builder.UseConfiguration(builtConfig);
    }
}
