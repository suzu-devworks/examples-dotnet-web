# Examples.Web.Authentication.Certificate

## Table of Contents <!-- omit in toc -->

- [Microsoft.AspNetCore.Authentication.Certificate](#microsoftaspnetcoreauthenticationcertificate)
- [Development](#development)
  - [Build](#build)
  - [Run](#run)
  - [How the project was initialized](#how-the-project-was-initialized)
- [References](#references)

## Microsoft.AspNetCore.Authentication.Certificate

Provides classes to support certificate authentication.

## Development

### Build

Build this project from the repository root:

```shell
dotnet build src/Examples.Web.Authentication.Certificate/
```

### Run

Run this project from the repository root:

```shell
dotnet run --project src/Examples.Web.Authentication.Certificate/ -lp https
```

### How the project was initialized

This project was initialized with the following commands:

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Authentication.Certificate
dotnet new webapp -o src/Examples.Web.Authentication.Certificate
dotnet sln add src/Examples.Web.Authentication.Certificate/
cd src/Examples.Web.Authentication.Certificate
dotnet add reference ../Examples.Web.Infrastructure/
dotnet add reference ../Examples.Web.Infrastructure.Assets/
dotnet add package Microsoft.AspNetCore.Authentication.Certificate

dotnet user-secrets init
cd ../../

# Check outdated packages
dotnet list package --outdated
```

## References

- [Configure certificate authentication in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/certauth)
