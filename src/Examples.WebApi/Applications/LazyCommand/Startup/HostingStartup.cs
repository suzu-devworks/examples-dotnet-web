using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Examples.WebApi.Applications.LazyCommand.Startup.HostingStartup))]

namespace Examples.WebApi.Applications.LazyCommand.Startup;

/// <summary>
/// Apply applications for specific configuration to <see cref="IWebHostBuilder" />.
/// </summary>
public class HostingStartup : IHostingStartup
{
    /// <summary>
    /// Configure the  <see cref="IWebHostBuilder" />.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services => services.AddLazyCommandApplication());

        return;
    }
}
