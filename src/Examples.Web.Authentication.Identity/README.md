# Examples.Web.Authentication.Identity

## Index

- [Scaffold Identity in ASP.NET Core projects](../../docs/auth/identity/identity_scaffold.md)
- [Identity model customization](../../docs/auth/identity/identity_customization.md)
- [OAuth 2.0 provider authentication](../../docs/auth/identity/identity_oauth2.md)
- [Two factor authentication ](../../docs/auth/identity/identity_2fa.md)
- [Account confirmation and password recovery](../../docs/auth/identity/identity_confirmation_email.md)

- [Web API backend for SPAs](../../docs/auth/identity/identity_spa_backend.md)
    - [Will 401 not be returned if it coexists with the webapp?](../../docs/auth/identity/identity_spa_backend.md#will-401-not-be-returned-if-it-coexists-with-the-webapp)


## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Authentication.Identity
dotnet new mvc -o src/Examples.Web.Authentication.Identity
dotnet sln add src/Examples.Web.Authentication.Identity/
cd src/Examples.Web.Authentication.Identity

### Add required NuGet package.
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Sqlite

dotnet add package Microsoft.AspNetCore.Authentication.Google
dotnet add package Microsoft.AspNetCore.Authentication.MicrosoftAccount
dotnet add package AspNet.Security.OAuth.GitHub

### Run the Identity scaffolds with the options you want
dotnet aspnet-codegenerator identity --useDefaultUI --databaseProvider 'sqlite'

### Create a migration and update the database. 
dotnet ef migrations add CreateIdentitySchema
dotnet ef database update

cd ../../

# Update outdated package
dotnet list package --outdated

# Tools
dotnet new tool-manifest
dotnet tool install dotnet-aspnet-codegenerator
dotnet tool restore
```
