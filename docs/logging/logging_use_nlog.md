# ASP.NET Core logging using NLog

## Table of Contents <!-- omit in toc -->

- [ASP.NET Core logging using NLog](#aspnet-core-logging-using-nlog)
  - [References](#references)
  - [Configuration file name](#configuration-file-name)
  - [Configurations](#configurations)
    - [Basic style in `program.cs`](#basic-style-in-programcs)

## References

- [NLog](https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-6)


##  Configuration file name

Reading configuration files with `LoadConfigurationFromAppSettings()`.

There are some differences depending on the environment variables:

```cs
environment = environment ?? GetAspNetCoreEnvironment("ASPNETCORE_ENVIRONMENT") ?? GetAspNetCoreEnvironment("DOTNET_ENVIRONMENT") ?? "Production";
basePath = basePath ?? GetAspNetCoreEnvironment("ASPNETCORE_CONTENTROOT") ?? GetAspNetCoreEnvironment("DOTNET_CONTENTROOT");
```

The order should be:

1. `appsettings.{environment}.json`
2. `appsettings.json`
3. `nlog.{environment}.config`
4. `nlog.config`

`appsettings.json` is merged, 
but `nlog.config` seems to be replaced.


## Configurations

### Basic style in `program.cs`

For now, this format is the norm.

```cs
// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    //# Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    //...
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
```

Considering that the `ILogger` interface will be called by other extension methods and libraries, I think it's a good idea to set up `builder.Host.UseNLog()` as soon as possible.

