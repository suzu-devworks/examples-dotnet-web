# Examples.Web.WebAPI

## Configurations

- [Logging use NLog](../../docs/logging/logging_use_nlog.md)
- [URLs case in controller routing](../../docs/routing/routing_controller_urls_case.md)
- [Urls Path base rewriting](../../docs/routing/routing_urls_rewruting.md)
- [Secure HTTP Response Header](../../docs/security/security_http_response_header.md)

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Web.WebAPI
dotnet new webapi --use-controllers -o src/Examples.Web.WebAPI
dotnet sln add src/Examples.Web.WebAPI/
cd src/Examples.Web.WebAPI
dotnet add reference ../Examples.Web.Infrastructure/
dotnet add package Swashbuckle.AspNetCore
dotnet add package NLog.Web.AspNetCore
dotnet add package NLog

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```