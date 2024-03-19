# Examples.Web.HostingStartup

## Table of Contents <!-- omit in toc -->

- [Examples.Web.HostingStartup](#exampleswebhostingstartup)
  - [References](#references)
  - [Creating Hosting startup assemblies](#creating-hosting-startup-assemblies)
    - [`IHostingStartup` implementation](#ihostingstartup-implementation)
    - [Configuration provided by the hosting startup](#configuration-provided-by-the-hosting-startup)
    - [`HostingStartup` attribute](#hostingstartup-attribute)
    - [Disable automatic loading of hosting startup assemblies](#disable-automatic-loading-of-hosting-startup-assemblies)
    - [Specify the hosting startup assembly](#specify-the-hosting-startup-assembly)
  - [Activation](#activation)

## References

- [Use hosting startup assemblies in ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-8.0)


## Creating Hosting startup assemblies

### `IHostingStartup` implementation

An `IHostingStartup` (hosting startup) implementation adds enhancements to an app at startup from an external assembly

```cs
using Microsoft.AspNetCore.Hosting;

public class StartupEnhancementHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        // ...
    }
}
```

### Configuration provided by the hosting startup

- When `ConfigureAppConfiguration` is used, hosting startup configuration to override app configuration.
- When `builder.UseConfiguration` is used, the app's configuration values take precedence over those specified by the hosting startup.

However, it seems that it will eventually be overwritten in WebApplicationBuilder.Configuration.


### `HostingStartup` attribute

A `HostingStartup` attribute indicates the presence of a hosting startup assembly to activate at runtime.

```cs
[assembly: HostingStartup(typeof(StartupEnhancementHostingStartup))]
```

### Disable automatic loading of hosting startup assemblies

To prevent all hosting startup assemblies from loading, set one of the following to true or 1:

- Prevent Hosting Startup host configuration setting:
    ```cs
        webHostBuilder.UseSetting(WebHostDefaults.PreventHostingStartupKey, "True");`
    ```
- `ASPNETCORE_PREVENTHOSTINGSTARTUP` environment variable.

To prevent specific hosting startup assemblies from loading, set one of the following to a semicolon-delimited string of hosting startup assemblies to exclude at startup:

- Hosting Startup Exclude Assemblies host configuration setting:
    ```cs
        webHostBuilder.UseSetting(
                WebHostDefaults.HostingStartupExcludeAssembliesKey, 
                "{ASSEMBLY1;ASSEMBLY2; ...}");
    ```
- `ASPNETCORE_HOSTINGSTARTUPEXCLUDEASSEMBLIES` environment variable.


### Specify the hosting startup assembly

For either a class library- or console app-supplied hosting startup, specify the hosting startup assembly's name in the `ASPNETCORE_HOSTINGSTARTUPASSEMBLIES` environment variable. The environment variable is a semicolon-delimited list of assemblies.

A hosting startup assembly can also be set using the Hosting Startup Assemblies host configuration setting:

```cs
    webHostBuilder.UseSetting(
            WebHostDefaults.HostingStartupAssembliesKey, 
            "{ASSEMBLY1;ASSEMBLY2; ...}");
```

## Activation

Options for hosting startup activation are:

- Runtime store
- Compile-time reference required for activation
    - NuGet package
    - Project bin folder

IHostingStartup, which is in the WebApp's own assembly, is automatically loaded.
