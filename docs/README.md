# examples-dotnet-web docs

## The way to the present

```shell
git clone https://github.com/suzu-devworks/examples-dotnet-webapi.git

dotnet new sln -o .

# Examples.Domain
dotnet new classlib -o src/Examples.Domain
dotnet sln add src/Examples.Domain/
dotnet add src/Examples.WebApi/Examples.WebApi.csproj reference src/Examples.Domain/
cd src/Examples.Domain
cd ../../

# Examples.Infrastructure
dotnet new classlib -o src/Examples.Infrastructure
dotnet sln add src/Examples.Infrastructure/
cd src/Examples.Infrastructure
cd ../../

## Examples.WebApi
dotnet new webapi -o src/Examples.WebApi
dotnet sln add src/Examples.WebApi/Examples.WebApi.csproj
cd src/Examples.WebApi
dotnet add reference ../Examples.Infrastructure/
cd ../../

## Examples.WebUI
dotnet new mvc -o src/Examples.WebUI
dotnet sln add src/Examples.WebUI/
cd src/Examples.WebUI
dotnet add reference ../Examples.Infrastructure/
## see auth/aspnetcore_identity.md

cd ../../

dotnet build

# Update outdated package
dotnet list package --outdated

## Tools
dotnet new tool-manifest
dotnet tool install dotnet-ef
dotnet tool install dotnet-aspnet-codegenerator
dotnet tool restore

```

### Referenced.

* https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/localization?view=aspnetcore-5.0
* https://docs.microsoft.com/ja-jp/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-5.0&tabs=visual-studio

