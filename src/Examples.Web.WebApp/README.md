# Examples.Web.WebApp

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [Examples Index](#examples-index)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Overview

This application is intended for testing and exploring the functionality of ASP.NET web applications (Razor).

## Examples Index

- Logging
  - [Logging using Microsoft.Extensions.Logging.Console](../../docs/logging/logging_use_console.md)
- Laboratories
  - [Filter methods for Razor Pages in ASP.NET Core](../../docs/filters/filters_in_razor_pages.md)
- Environments
  - [Use hosting startup assemblies in ASP.NET Core](../../docs/environments/hosting_startup.md)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Infrastructure
dotnet new classlib -o src/Examples.Web.Infrastructure
dotnet sln add src/Examples.Web.Infrastructure/
cd src/Examples.Web.Infrastructure
cd ../../

## Examples.Web.Examples.Web.Infrastructure.Startup
dotnet new classlib -o src/Examples.Web.Infrastructure.Startup
dotnet sln add src/Examples.Web.Infrastructure.Startup/
cd src/Examples.Web.Infrastructure.Startup
cd ../../

## Examples.Web.WebApp
dotnet new webapp -o src/Examples.Web.WebApp
dotnet sln add src/Examples.Web.WebApp/
cd src/Examples.Web.WebApp
dotnet add reference ../Examples.Web.Infrastructure
dotnet add reference ../Examples.Web.Infrastructure.Startup

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```
