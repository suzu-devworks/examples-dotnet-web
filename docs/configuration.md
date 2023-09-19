# Configuration

## The way to the present

```shell
git clone https://github.com/suzu-devworks/examples-dotnet-web.git
cd examples-dotnet-web

## run in Dev Container.

dotnet new sln -o .

#dotnet nuget update source github --username suzu-devworks --password "{parsonal access token}" --store-password-in-clear-text

## Examples.Web.Infrastructure
dotnet new classlib -o src/Examples.Web.Infrastructure
dotnet sln add src/Examples.Web.Infrastructure/
cd src/Examples.Web.Infrastructure
cd ../../

## Examples.DependencyInjection.Tests
dotnet new xunit -o src/Examples.Web.Infrastructure.Tests
dotnet sln add src/Examples.Web.Infrastructure.Tests/
cd src/Examples.Web.Infrastructure.Tests
dotnet add reference ../Examples.Web.Infrastructure
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit
cd ../../

## Examples.WebAPI
dotnet new webapi -o src/Examples.WebAPI
dotnet sln add src/Examples.WebAPI/
cd src/Examples.WebAPI
dotnet add reference ../Examples.Web.Infrastructure/
dotnet add package NLog.Web.AspNetCore
dotnet add package NLog
cd ../../

dotnet build


# Update outdated package
dotnet list package --outdated

# Tools
dotnet new tool-manifest
dotnet tool install coverlet.console

```
