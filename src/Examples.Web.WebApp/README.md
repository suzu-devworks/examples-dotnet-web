# Examples.Web.WebApp

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [Examples](#examples)
  - [Logging](#logging)
  - [Environments](#environments)
    - [A scenario for adding modules through Hosting Startup](#a-scenario-for-adding-modules-through-hosting-startup)
  - [Laboratories](#laboratories)
    - [Learn the order of filter execution and how to use them](#learn-the-order-of-filter-execution-and-how-to-use-them)
- [Development](#development)
  - [Build](#build)
  - [Run](#run)
  - [How the project was initialized](#how-the-project-was-initialized)

## Overview

This application is intended for testing and exploring the functionality of ASP.NET web applications (Razor).

## Examples

### Logging

- [Can the console logs look a bit better?](../../docs/logging/logging_use_console.md#tiny-colored-console)
- [Aren't the logs being output twice?](../../docs/logging/logging_use_console.md#the-log-is-generated-twice)

### Environments

#### A scenario for adding modules through Hosting Startup

- [Use hosting startup assemblies in ASP.NET Core](../../docs/hosting/hosting_startup.md)

Set up the following environments, override environment variables in each one, and verify the order of precedence.

- ASP.NET (this) Startup | [Program.cs](./Program.cs)
- ASP.NET (this) IHostingStartup | [MyHostingStartup.cs](./Applications/Environments/MyHostingStartup.cs)
- Other Library 1 IHostingStartup | [PriorityViewHostingStartup.cs](../fixtures/Examples.Web.HostingStartup1/PriorityViewHostingStartup.cs)
- Other Library 2 IHostingStartup | [PriorityViewHostingStartup.cs](../fixtures/Examples.Web.HostingStartup2/PriorityViewHostingStartup.cs)

In practice, each library would likely provide its own extension methods, so this section does not go too deep.

### Laboratories

#### Learn the order of filter execution and how to use them

- [Filter methods for Razor Pages in ASP.NET Core](../../docs/filters/filters_in_razor_pages.md)

Prepare the following action buttons and use logs to verify how each filter type is defined and in what order they run.

- Normal behavior (Action returns OK)まちが
- Exception behavior (Action throws)
- Short-circuit behavior

## Development

### Build

Build this project from the repository root:

```shell
dotnet build src/Examples.Web.WebApp/Examples.Web.WebApp.csproj
```

### Run

Run this project from the repository root:

```shell
dotnet run --project src/Examples.Web.WebApp/Examples.Web.WebApp.csproj
```

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

## Examples.Web.HostingStartup1
dotnet new classlib -o src/fixtures/Examples.Web.HostingStartup1
dotnet sln add src/fixtures/Examples.Web.HostingStartup1/
cd src/fixtures/Examples.Web.HostingStartup1
cd ../../../

## Examples.Web.HostingStartup2
dotnet new classlib -o src/fixtures/Examples.Web.HostingStartup2
dotnet sln add src/fixtures/Examples.Web.HostingStartup2/
cd src/fixtures/Examples.Web.HostingStartup2
cd ../../../

## Examples.Web.WebApp
dotnet new webapp -o src/Examples.Web.WebApp
dotnet sln add src/Examples.Web.WebApp/
cd src/Examples.Web.WebApp
dotnet add reference ../Examples.Web.Infrastructure
dotnet add reference ../fixtures/Examples.Web.HostingStartup1
dotnet add reference ../fixtures/Examples.Web.HostingStartup2

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```
