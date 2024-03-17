# Examples.Web.Authentication.Basic


## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Authentication.Basic
dotnet new webapp -o src/Examples.Web.Authentication.Basic
dotnet sln add src/Examples.Web.Authentication.Basic/
cd src/Examples.Web.Authentication.Basic

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```

