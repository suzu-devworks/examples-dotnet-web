# Scaffold Identity in ASP.NET Core projects

## Table of Contents <!-- omit in toc -->

- [Scaffold Identity in ASP.NET Core projects](#scaffold-identity-in-aspnet-core-projects)
  - [References](#references)
  - [Scaffold Identity into an MVC project without existing authorization.](#scaffold-identity-into-an-mvc-project-without-existing-authorization)
    - [Add tools.](#add-tools)
    - [Add required NuGet package.](#add-required-nuget-package)
    - [Run the Identity scaffolder with the options you want.](#run-the-identity-scaffolder-with-the-options-you-want)
    - [Create a migration and update the database.](#create-a-migration-and-update-the-database)
    - [Modify Code on MVC](#modify-code-on-mvc)
    - [Authorize View](#authorize-view)


## References

- [Scaffold Identity in ASP.NET Core projects | Microsoft Learn](https://docs.microsoft.com/ja-jp/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-6.0&tabs=visual-studio)


## Scaffold Identity into an MVC project without existing authorization.

### Add tools.

```shell
dotnet new tool-manifest
dotnet tool install dotnet-ef
dotnet tool install dotnet-aspnet-codegenerator
dotnet tool restore
```

```shell
dotnet aspnet-codegenerator identity -h
```

### Add required NuGet package.

```shell
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### Run the Identity scaffolder with the options you want.

```shell
dotnet aspnet-codegenerator identity --useDefaultUI --databaseProvider 'sqlite'
```

### Create a migration and update the database. 

```shell
dotnet ef migrations add CreateIdentitySchema
dotnet ef database update
```

```shell
dotnet ef migrations list
```

### Modify Code on MVC

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

**Program.cs**

```diff
@@ -23,5 +31,6 @@
 app.MapControllerRoute(
     name: "default",
     pattern: "{controller=Home}/{action=Index}/{id?}");
+app.MapRazorPages();
 
 app.Run();
```

### Authorize View

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