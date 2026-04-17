# ASP.NET Core logging using NLog

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [Add dependency](#add-dependency)
- [Loading Nlog Configuration](#loading-nlog-configuration)
- [Prioritization of configuration files](#prioritization-of-configuration-files)
- [Set up `Program.cs`](#set-up-programcs)
- [References](#references)

## Overview

With containers, outputting to the console is often sufficient,
but Nlog is still necessary when file logging or special logging is required.

## Add dependency

Update the `NLog` package if possible

```shell
dotnet add package NLog.Web.AspNetCore
dotnet add package NLog
```

## Loading Nlog Configuration

When loading from `appsettings.json`, use `LoadConfigurationFromAppSettings()`.
You can also specify the node to load from.

When loading from a different file named `nlog.config`, use `LoadConfigurationFromFile()`.

When loading settings from a source other than `appsettings.json`, it is best to first create a Configuration and then use `LoadConfigurationFromSection()`.

When using an XML string, it seems best to initialize with `LoadConfigurationFromXml()`.

## Prioritization of configuration files

When loading a configuration file using `LoadConfigurationFromAppSettings()`

The order should be:

1. `appsettings.{environment}.json`
2. `appsettings.json`
3. `nlog.{environment}.config`
4. `nlog.config`

The settings should already be integrated, so you should only need to write the differences (hopefully).

## Set up `Program.cs`

For now, this format is the norm.

```cs
// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Set up().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    //# Set up NLog for Dependency injection
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

## References

- [NLog](https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-6)
- [Config |  NLog](https://nlog-project.org/config/)
