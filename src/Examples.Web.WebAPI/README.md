# Examples.Web.WebAPI


## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Web.WebAPI
dotnet new webapi --use-controllers -o src/Examples.Web.WebAPI
dotnet sln add src/Examples.Web.WebAPI/
cd src/Examples.Web.WebAPI
dotnet add package Swashbuckle.AspNetCore
cd ../../

# Update outdated package
dotnet list package --outdated
```
