# Examples.Web.Authentication.Identity

## References

- [Scaffold Identity into an MVC project without existing authorization ...](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-8.0&tabs=netcore-cli#scaffold-identity-into-an-mvc-project-without-existing-authorization)
- [qrcode.js](https://davidshimjs.github.io/qrcodejs/


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

### Run the Identity scaffolder with the options you want
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
