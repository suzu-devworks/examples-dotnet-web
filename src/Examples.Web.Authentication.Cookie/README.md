# Examples.Web.Authentication.Cookie

## Table of Contents <!-- omit in toc -->

## Microsoft.AspNetCore.Authentication.Cookies

Contains types that support cookie based authentication.

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Authentication.Cookie
dotnet new webapp -o src/Examples.Web.Authentication.Cookie
dotnet sln add src/Examples.Web.Authentication.Cookie/
cd src/Examples.Web.Authentication.Cookie
dotnet add reference ../Examples.Web.Infrastructure/

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```

## References

- [Use cookie authentication without ASP.NET Core Identity](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/cookie)
