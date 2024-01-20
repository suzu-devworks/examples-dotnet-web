# ASP.NET Core Identity

ASP.NET Core Identity:

- Is an API that supports user interface (UI) login functionality.
- Manages users, passwords, profile data, roles, claims, tokens, email confirmation, and more.


## Table of contents

- [Customization](./identity_customization.md)
- [Configuration](./identity_configuration.md)
- [External provider](./identity_external_provider.md)
- [Two factor authentication](./identity_2fa.md)
- [Confirmation email and reset password](./identity_confirmation_email.md)
- [Web API backend for SPAs](./identity_spa.md)

## References

- [Introduction to Identity on ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/identity?view=aspnetcore-8.0&tabs=netcore-cli)


## Scaffold Identity in ASP.NET Core projects

### Add tools.

```shell
dotnet new tool-manifest
dotnet tool install dotnet-ef
dotnet tool install dotnet-aspnet-codegenerator
```

restore tool:

```shell
dotnet tool restore
```

show help:

```shell
dotnet aspnet-codegenerator identity -h
```

### Add required NuGet package.

```shell
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.Tools

dotnet add package Microsoft.EntityFrameworkCore.SqlServer
# or
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```


### Scaffold your identity with the options you want.

To set up identity with a default UI and minimal files, run the following command:

```shell
dotnet aspnet-codegenerator identity --useDefaultUI --databaseProvider 'sqlite'
```

To create the specified Razor page and set its ID, run the following command:

```shell
dotnet aspnet-codegenerator identity -dc Examples.Web.Authentication.Identity.Areas.Identity.Data.IdentityDataContext --files 'Account.Register;Account.Login;Account.Logout;Account.RegisterConfirmation;Account.ResetPassword' --databaseProvider 'sqlite'
```

Check how to specify `--files` with the following command:

```shell
dotnet aspnet-codegenerator identity --listFiles
```

If you want all UI files, do not specify anything:

```shell
dotnet aspnet-codegenerator identity -dc Examples.Web.Authentication.Identity.Areas.Identity.Data.IdentityDataContext --databaseProvider 'sqlite'
```

### Migrations

The generated Identity database code requires Entity Framework Core Migrations. For example, run the following commands:

```shell
dotnet ef migrations add CreateIdentitySchema
dotnet ef database update
```

only the command to update the database must be executed:

```shell
dotnet ef database update
```

You can confirm the application of an Identity schema with the following command:

```shell
dotnet ef migrations list
```

### Setup startup code

Add MapRazorPages to `Program.cs`:

```diff
@@ -23,5 +31,6 @@
 app.MapControllerRoute(
     name: "default",
     pattern: "{controller=Home}/{action=Index}/{id?}");
+app.MapRazorPages();
 
 app.Run();
```

### (Optional)Setup Razor Layout

Add the login partial (_LoginPartial) to the Views/Shared/_Layout.cshtml file:

**/Views/Shared/\_Layout.cshtml**

```diff
@@ -30,6 +30,8 @@
                                 asp-action="Privacy">Privacy</a>
                         </li>
                     </ul>
+                    <!-- ADD Identity -->
+                    <partial name="_LoginPartial" />
                 </div>
             </div>
         </nav>
```

### Setup Authorize View

**/Controllers/HomeController.cs**

```diff
@@ -1,5 +1,6 @@
using System.Diagnostics;
using Examples.WebUI.Authentication.Models;

+using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examples.WebUI.Authentication.Controllers;
@@ -18,6 +19,7 @@ public IActionResult Index()
         return View();
     }

+    [Authorize]
     public IActionResult Privacy()
     {
         return View();
```

