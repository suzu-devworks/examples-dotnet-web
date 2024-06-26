# Examples.Web.WebUI

## Index

- [Tiny colored console](../../docs/logging/logging_use_console.md)
- [Filter methods for Razor Pages in ASP.NET Core](../../docs/filters/filters_in_razor_pages.md)
- [Hosting startup assemblies](../Examples.Web.HostingStartup/README.md)


## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Infrastructure
dotnet new classlib -o src/Examples.Web.Infrastructure
dotnet sln add src/Examples.Web.Infrastructure/
cd src/Examples.Web.Infrastructure
dotnet add package Swashbuckle.AspNetCore
dotnet add package Swashbuckle.AspNetCore.Annotations
cd ../../

## Examples.Web.HostingStartup
dotnet new classlib -o src/Examples.Web.HostingStartup
dotnet sln add src/Examples.Web.HostingStartup/
cd src/Examples.Web.HostingStartup
cd ../../

## Examples.Web.WebUI
dotnet new webapp -o src/Examples.Web.WebUI
dotnet sln add src/Examples.Web.WebUI/
cd src/Examples.Web.WebUI
dotnet add reference ../Examples.Web.Infrastructure
dotnet add reference ../Examples.Web.HostingStartup

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```
