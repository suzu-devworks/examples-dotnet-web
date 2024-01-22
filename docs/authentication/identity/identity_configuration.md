# ASP.NET Core Identity Configuration

## References

- [Configure ASP.NET Core Identity ...](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-8.0)


## Scaffold UI files

```shell
dotnet aspnet-codegenerator identity -dc Examples.Web.Authentication.Identity.Areas.Identity.Data.IdentityDataContext --files 'Account.Register;Account.Login;Account.Logout;Account.RegisterConfirmation;Account.ResetPassword' --databaseProvider 'sqlite'
```

### `error CS0121: The call is ambiguous between the following methods or properties`

When using an extension method to extract the "Identity" settings part of "Program.cs", the following error may occur:

```console
Building project ...
Finding the generator 'identity'...
Running the generator 'identity'...
Failed to compile the project in memory
/workspaces/examples-dotnet-web/src/Examples.Web.Authentication.Identity/Program.cs(8,18): error CS0121: The call is ambiguous between the following methods or properties: 'Examples.Web.Infrastructure.Authentication.Identity.ServiceCollectionExtensions.AddIdentityAuthentication(Microsoft.Extensions.DependencyInjection.IServiceCollection, System.Action<Examples.Web.Infrastructure.Authentication.Identity.ServiceCollectionExtensions.ConfigureOption>)' and 'Examples.Web.Infrastructure.Authentication.Identity.ServiceCollectionExtensions.AddIdentityAuthentication(Microsoft.Extensions.DependencyInjection.IServiceCollection, System.Action<Examples.Web.Infrastructure.Authentication.Identity.ServiceCollectionExtensions.ConfigureOption>)'
   at Microsoft.VisualStudio.Web.CodeGeneration.ActionInvoker.<BuildCommandLine>b__6_0()
   at Microsoft.Extensions.CommandLineUtils.CommandLineApplication.Execute(String[] args)
   at Microsoft.VisualStudio.Web.CodeGeneration.ActionInvoker.Execute(String[] args)
   at Microsoft.VisualStudio.Web.CodeGeneration.CodeGenCommand.Execute(String[] args)
```

In this case, temporarily comment out the extension method part and you will be able to generate the scaffold.



## Use Lockout

- [Lockout ...](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-8.0#lockout)

**LockoutOptions**

| プロパティ| 説明 | Default | 
| ---------------------- | -------------------------------------------- | ------- |
| AllowedForNewUsers      | 新しいユーザーをロックアウトできるかどうかを判断します。| true|
| DefaultLockoutTimeSpan  | ロックアウトが発生した場合にユーザーがロックアウトされる時間。| 5 minutes |
| MaxFailedAccessAttempts | ロックアウトが有効になっている場合に、ユーザーがロックアウトされるまでに失敗したアクセス試行回数。 | 5 |


```diff
--- a/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/Login.cshtml.cs
+++ b/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/Login.cshtml.cs
@@ -104,7 +104,7 @@ public async Task<IActionResult> OnPostAsync(string returnUrl = null)
             {
                 // This doesn't count login failures towards account lockout
                 // To enable password failures to trigger account lockout, set lockoutOnFailure: true
-                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
+                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                 if (result.Succeeded)
                 {
                     _logger.LogInformation("User logged in.");
```

```diff
--- a/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
+++ b/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
@@ -27,6 +27,14 @@ public static class ServiceCollectionExtensions
         services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
             .AddEntityFrameworkStores<IdentityDataContext>();
 
+        services.Configure<IdentityOptions>(options =>
+        {
+            // Default Lockout settings.
+            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
+            options.Lockout.MaxFailedAccessAttempts = 5;
+            options.Lockout.AllowedForNewUsers = true;
+        });
+
         return services;
     }
```


### Password Policy

- [Password ...](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-8.0#password)

| プロパティ             | 説明                                         | Default |
| ---------------------- | -------------------------------------------- | ------- |
| RequireDigit           | 数値[0-9]を必要とします。                    | true    |
| RequireLowercase       | 小文字[a-z]を必要とします。                  | true    |
| RequireUppercase       | 大文字[A-Z]を必要とします。                  | true    |
| RequireNonAlphanumeric | 英数字以外の文字を必要とします。             | true    |
| RequiredLength         | 最低限の長さ。                               | 6       |
| RequiredUniqueChars    | 個別の文字の数を必要とします。<br>※0000 防止 | 1       |

```diff
--- a/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
+++ b/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
@@ -33,6 +33,23 @@ public static class ServiceCollectionExtensions
             options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
             options.Lockout.MaxFailedAccessAttempts = 5;
             options.Lockout.AllowedForNewUsers = true;
+
+            // // Default Password settings.
+            // options.Password.RequireDigit = true;
+            // options.Password.RequireLowercase = true;
+            // options.Password.RequireNonAlphanumeric = true;
+            // options.Password.RequireUppercase = true;
+            // options.Password.RequiredLength = 6;
+            // options.Password.RequiredUniqueChars = 1;
+
+            // Weak password settings.
+            options.Password.RequireDigit = false;
+            options.Password.RequireLowercase = false;
+            options.Password.RequireUppercase = false;
+            options.Password.RequireNonAlphanumeric = false;
+            options.Password.RequiredLength = 4;
+            options.Password.RequiredUniqueChars = 2;
+
         });
 
         return services;
```

> Don't forget the attribute of model.

```diff
--- a/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/Register.cshtml.cs
+++ b/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/Register.cshtml.cs
@@ -77,7 +77,7 @@ public class InputModel
             ///     directly from your code. This API may change or be removed in future releases.
             /// </summary>
             [Required]
-            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
+            [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
             [DataType(DataType.Password)]
             [Display(Name = "Password")]
             public string Password { get; set; }
```

```diff
--- a/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/ResetPassword.cshtml.cs
+++ b/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/ResetPassword.cshtml.cs
@@ -46,7 +46,7 @@ public class InputModel
             ///     directly from your code. This API may change or be removed in future releases.
             /// </summary>
             [Required]
-            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
+            [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
             [DataType(DataType.Password)]
             public string Password { get; set; }
```

### Sign-in

-[Sign-in ...](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-8.0#sign-in)

```diff
--- a/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
+++ b/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
@@ -50,6 +50,10 @@ public static class ServiceCollectionExtensions
             options.Password.RequiredLength = 4;
             options.Password.RequiredUniqueChars = 2;
 
+            // Default SignIn settings.
+            options.SignIn.RequireConfirmedEmail = false;
+            options.SignIn.RequireConfirmedPhoneNumber = false;
+
         });
 
         return services;
```


### User

- [User ...](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-8.0#user)

```diff
--- a/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
+++ b/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
@@ -54,6 +54,11 @@ public static class ServiceCollectionExtensions
             options.SignIn.RequireConfirmedEmail = false;
             options.SignIn.RequireConfirmedPhoneNumber = false;
 
+            // Default User settings.
+            options.User.AllowedUserNameCharacters =
+                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
+            options.User.RequireUniqueEmail = false;
+
         });
 
         return services;
```


### Cookie settings

- [Cookie settings ...](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-8.0#cookie-settings)

```diff
--- a/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
+++ b/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
@@ -1,6 +1,7 @@
 using Microsoft.AspNetCore.Identity;
 using Microsoft.EntityFrameworkCore;
 using Examples.Web.Authentication.Identity.Areas.Identity.Data;
+using Microsoft.AspNetCore.Authentication.Cookies;
 
 namespace Examples.Web.Infrastructure.Authentication.Identity;
 
@@ -61,6 +62,19 @@ public static class ServiceCollectionExtensions
 
         });
 
+        services.ConfigureApplicationCookie(options =>
+        {
+            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
+            options.Cookie.Name = "YourAppCookieName";
+            options.Cookie.HttpOnly = true;
+            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
+            options.LoginPath = "/Identity/Account/Login";
+            // ReturnUrlParameter requires Microsoft.AspNetCore.Authentication.Cookies;
+            //using Microsoft.AspNetCore.Authentication.Cookies;
+            options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
+            options.SlidingExpiration = true;
+        });
+
         return services;
     }
```
