# Examples.Web.WebAPI

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Infrastructure
dotnet new webapi -o src/Examples.Web.WebAPI
dotnet sln add src/Examples.Web.WebAPI/
cd src/Examples.Web.WebAPI
dotnet add reference ../../src/Examples.Web.Infrastructure
cd ../../

# Update outdated package
dotnet list package --outdated
```
