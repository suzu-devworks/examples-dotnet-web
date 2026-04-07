# Use hosting startup assemblies in ASP.NET Core

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
  - [HostingStartup attribute](#hostingstartup-attribute)
  - [`IHostingStartup` implementation](#ihostingstartup-implementation)
  - [project types](#project-types)
  - [To find the loaded hosting startup assemblies, enable logging and check your app's logs](#to-find-the-loaded-hosting-startup-assemblies-enable-logging-and-check-your-apps-logs)
- [Which should take priority](#which-should-take-priority)
  - [Disable automatic loading of hosting startup assemblies](#disable-automatic-loading-of-hosting-startup-assemblies)
  - [Specify the hosting startup assembly](#specify-the-hosting-startup-assembly)
- [References](#references)

## Overview

An IHostingStartup (hosting startup) implementation adds enhancements to an app at startup from an external assembly. For example, an external library can use a hosting startup implementation to provide additional configuration providers or services to an app.

### HostingStartup attribute

A HostingStartup attribute indicates the presence of a hosting startup assembly to activate at runtime.

```cs
[assembly: HostingStartup(typeof(StartupEnhancement.StartupEnhancementHostingStartup))]
```

### `IHostingStartup` implementation

An `IHostingStartup` (hosting startup) implementation adds enhancements to an app at startup from an external assembly

```cs
using Microsoft.AspNetCore.Hosting;

public class StartupEnhancementHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
      // Use the IWebHostBuilder to add app enhancements.
    }
}
```

### project types

Create a hosting startup with either of the following project types:

- Class library
- Console app without an entry point

The class library retains the same characteristics even when referenced as a NuGet package.

The `Console app without an entry point` method is used to create a `HostingStartup` that doesn't require compile-time references, but it requires registration with an unsupported runtime package store, so we won't cover it here. You probably won't need it anyway.

### To find the loaded hosting startup assemblies, enable logging and check your app's logs

in `appsettings.json`:

```diff
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
+     "Microsoft.AspNetCore.Hosting.Diagnostics": "Debug"
    },
    "Debug": {
      "LogLevel": {
        "Default": "None"
      }
    }
  }
```

## Which should take priority

- When `builder.ConfigureAppConfiguration()` is used, hosting startup configuration to override app configuration.

- When `builder.UseConfiguration()` is used, the app's configuration values take precedence over those specified by the hosting startup.

However, it seems that it will eventually be overwritten in WebApplicationBuilder.Configuration.

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

## References

- [Use hosting startup assemblies in ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-8.0)
