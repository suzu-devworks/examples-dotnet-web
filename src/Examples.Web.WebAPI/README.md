# Examples.Web.WebAPI

## Reference

- [NLog ...](https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-6)
- [UsePathBase ...](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.builder.usepathbaseextensions.usepathbase?view=aspnetcore-8.0)
    - It was different than I expected. <br/>
No base path is added to the routing path.
It seems to remove the base path from the request and then perform the routing
- [Kestrel web server ...](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/servers/kestrel/options?view=aspnetcore-8.0)
    - Isn't it automatically configured with ConfigureKestrel?
- [Secure Response Header ...](../../docs/security/security_http_header.md)
- [Serialization ...](https://learn.microsoft.com/ja-jp/dotnet/standard/serialization/system-text-json/converters-how-to?pivots=dotnet-8-0)

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Infrastructure
dotnet new webapi -o src/Examples.Web.WebAPI
dotnet sln add src/Examples.Web.WebAPI/
cd src/Examples.Web.WebAPI
dotnet add reference ../../src/Examples.Web.Infrastructure
dotnet add package NLog.Web.AspNetCore
dotnet add package NLog
cd ../../

# Update outdated package
dotnet list package --outdated
```
