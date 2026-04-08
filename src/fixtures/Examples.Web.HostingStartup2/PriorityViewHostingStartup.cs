using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

[assembly: HostingStartup(typeof(Examples.Web.HostingStartup2.PriorityViewHostingStartup))]

namespace Examples.Web.HostingStartup2;

public class PriorityViewHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Demo:Source:HostingStartup2:ConfigurePattern"] = "Defined in HostingStartup2",
                ["Demo:OrderProbe:All"] = "HostingStartup2 ConfigureAppConfiguration",
                ["Demo:OrderProbe:ConfigurePattern"] = "HostingStartup2 ConfigureAppConfiguration",
                ["Demo:Compare:HostingStartup2"] = "From HostingStartup2 ConfigureAppConfiguration",
                ["Demo:Compare:ConfigureVsAppSettings"] = "From HostingStartup2 ConfigureAppConfiguration",
                ["Demo:Compare:ConfigureVsProgramConfigManager"] = "From HostingStartup2 ConfigureAppConfiguration",
                ["Demo:Compare:AssemblyOrder:ConfigureAppConfiguration"] = "From HostingStartup2 ConfigureAppConfiguration",
            }));

        var builtConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Demo:Source:HostingStartup2:UsePattern"] = "Defined in HostingStartup2",
                ["Demo:OrderProbe:All"] = "HostingStartup2 UseConfiguration",
                ["Demo:OrderProbe:UsePattern"] = "HostingStartup2 UseConfiguration",
                ["Demo:Compare:HostingStartup2"] = "From HostingStartup2 UseConfiguration",
                ["Demo:Compare:UseVsAppSettings"] = "From HostingStartup2 UseConfiguration",
                ["Demo:Compare:UseVsProgramConfigManager"] = "From HostingStartup2 UseConfiguration",
                ["Demo:Compare:AssemblyOrder:UseConfiguration"] = "From HostingStartup2 UseConfiguration",
            })
            .Build();

        builder.UseConfiguration(builtConfig);
    }
}
