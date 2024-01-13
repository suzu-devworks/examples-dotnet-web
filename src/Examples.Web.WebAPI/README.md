# Examples.Web.WebAPI

## Reference

- [NLog](https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-6)


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
