# Examples.Web.Authentication.Basic

## References

- [AspNetCore.Authentication.Basic ...](https://github.com/mihirdilip/aspnetcore-authentication-basic/tree/7.0.0)

## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Authentication.Basic
dotnet new webapp -o src/Examples.Web.Authentication.Basic
dotnet sln add src/Examples.Web.Authentication.Basic/
cd src/Examples.Web.WebAPI
dotnet add package AspNetCore.Authentication.Basic
dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```

