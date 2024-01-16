# ASP.NET Core Identity

ASP.NET Core Identity:

- ユーザー インターフェイス (UI) ログイン機能をサポートする API です。
- ユーザー、パスワード、プロファイル データ、ロール、要求、トークン、電子メールの確認などを管理します。

<!-- ----- -->

## References

- [ASP.NET Core プロジェクトでの Identity のスキャフォールディング - Microsoft Docs](https://docs.microsoft.com/ja-jp/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-6.0&tabs=visual-studio)
- [ASP.NET Core の Identity の概要](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio)

<!-- ----- -->

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

<!-- ----- -->

## Configuration

### Scaffold UI files

```shell
dotnet aspnet-codegenerator identity -dc Examples.Web.Authentication.Identity.Areas.Identity.Data.IdentityDataContext --files 'Account.Register;Account.Login;Account.Logout;Account.RegisterConfirmation;Account.ResetPassword' --databaseProvider 'sqlite'
```

`Program.cs` の `Identity` 設定部分を拡張メソッドで切り出した場合、次のエラーが出ることがあります。

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

この場合には一時的に拡張メソッド部分をコメントアウトすると、Scaffold を生成できるようになります。


### Naming Login

**/Areas/Identity/Pages/Account/Login.cshtml.cs**

```diff
@@ -59,8 +59,7 @@ public class InputModel
             ///     directly from your code. This API may change or be removed in future releases.
             /// </summary>
             [Required]
-            [EmailAddress]
-            public string Email { get; set; }
+            public string Name { get; set; }

             /// <summary>
             ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
@@ -105,7 +104,7 @@ public async Task<IActionResult> OnPostAsync(string returnUrl = null)
             {
                 // This doesn't count login failures towards account lockout
                 // To enable password failures to trigger account lockout, set lockoutOnFailure: true
-                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
+                var result = await _signInManager.PasswordSignInAsync(Input.Name, Input.Password, Input.RememberMe, lockoutOnFailure: false
);
                 if (result.Succeeded)
                 {
                     _logger.LogInformation("User logged in.");
```

**/Areas/Identity/Pages/Account/Login.cshtml**

```diff
@@ -14,9 +14,9 @@ <h2>Use a local account to log in.</h2>
                 <hr />
                 <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                 <div class="form-floating">
-                    <input asp-for="Input.Email" class="form-control" autocomplete="username" aria-required="true" />
-                    <label asp-for="Input.Email" class="form-label"></label>
-                    <span asp-validation-for="Input.Email" class="text-danger"></span>
+                    <input asp-for="Input.Name" class="form-control" autocomplete="username" aria-required="true" />
+                    <label asp-for="Input.Name" class="form-label"></label>
+                    <span asp-validation-for="Input.Name" class="text-danger"></span>
                 </div>
                 <div class="form-floating">
                     <input asp-for="Input.Password" class="form-control" autocomplete="current-password"
@@ -70,7 +70,7 @@ <h3>Use another service to log in.</h3>
                 class="form-horizontal">
                         <div>
                             <p>
-                                @foreach (var provider in Model.ExternalLogins)
+                                @foreach (var provider in Model.ExternalLogins!)
                                 {
                                     <button type="submit" class="btn btn-primary" name="provider" value="@provider.Name"
                                 title="Log in using your @provider.DisplayName account">@provider.DisplayName</button>
```

**/Areas/Identity/Pages/Account/Register.cshtml.cs**

```diff
@@ -4,14 +4,11 @@
 #pragma warning disable IDE0037

using System.ComponentModel.DataAnnotations;

-
using System.Text;

-using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

-
using Microsoft.AspNetCore.WebUtilities;

namespace Examples.WebUI.Authentication.Areas.Identity.Pages.Account
 {
@@ -69,9 +66,8 @@ public class InputModel
             ///     directly from your code. This API may change or be removed in future releases.
             /// </summary>
             [Required]
-            [EmailAddress]
-            [Display(Name = "Email")]
-            public string Email { get; set; }
+            [Display(Name = "Name")]
+            public string Name { get; set; }

             /// <summary>
             ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
@@ -107,32 +103,15 @@ public async Task<IActionResult> OnPostAsync(string returnUrl = null)
             if (ModelState.IsValid)
             {
                 var user = CreateUser();
+                user.EmailConfirmed = true;

-                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
-                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
+                await _userStore.SetUserNameAsync(user, Input.Name, CancellationToken.None);
                 var result = await _userManager.CreateAsync(user, Input.Password);

                 if (result.Succeeded)
                 {
                     _logger.LogInformation("User created a new account with password.");

-                    var userId = await _userManager.GetUserIdAsync(user);
-                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
-                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
-                    var callbackUrl = Url.Page(
-                        "/Account/ConfirmEmail",
-                        pageHandler: null,
-                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
-                        protocol: Request.Scheme);
-
-                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
-                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
-
-                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
-                    {
-                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
-                    }
-                    else
                     {
                         await _signInManager.SignInAsync(user, isPersistent: false);
                         return LocalRedirect(returnUrl);
```

**Areas/Identity/Pages/Account/Register.cshtml**

```diff
@@ -13,10 +13,11 @@ <h2>Create a new account.</h2>
             <hr />
             <div asp-validation-summary="ModelOnly" class="text-danger"></div>
             <div class="form-floating">
-                <input asp-for="Input.Email" class="form-control" autocomplete="username" aria-required="true" />
-                <label asp-for="Input.Email"></label>
-                <span asp-validation-for="Input.Email" class="text-danger"></span>
+                <input asp-for="Input.Name" class="form-control" autocomplete="username" aria-required="true" />
+                <label asp-for="Input.Name"></label>
+                <span asp-validation-for="Input.Name" class="text-danger"></span>
             </div>
+
             <div class="form-floating">
                 <input asp-for="Input.Password" class="form-control" autocomplete="new-password" aria-required="true" />
                 <label asp-for="Input.Password"></label>
@@ -52,7 +53,7 @@ <h3>Use another service to register.</h3>
                 class="form-horizontal">
                         <div>
                             <p>
-                                @foreach (var provider in Model.ExternalLogins)
+                                @foreach (var provider in Model.ExternalLogins!)
                                 {
                                     <button type="submit" class="btn btn-primary" name="provider" value="@provider.Name"
                                 title="Log in using your @provider.DisplayName account">@provider.DisplayName</button>
```


### Lockout

- [Lockout](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0#lockout)

**LockoutOptions**

| プロパティ| 説明 | Default | 
| ---------------------- | -------------------------------------------- | ------- |
| AllowedForNewUsers      | 新しいユーザーをロックアウトできるかどうかを判断します。| true|
| DefaultLockoutTimeSpan  | ロックアウトが発生した場合にユーザーがロックアウトされる時間。| 5 minutes |
| MaxFailedAccessAttempts | ロックアウトが有効になっている場合に、ユーザーがロックアウトされるまでに失敗したアクセス試行回数。 | 5 |

**Areas/Identity/Pages/Account/Login.cshtml.cs**

```diff
@@ -111,7 +111,8 @@ public async Task<IActionResult> OnPostAsync(string returnUrl = null)
             {
                 // This doesn't count login failures towards account lockout
                 // To enable password failures to trigger account lockout, set lockoutOnFailure: true
-                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: 
false);
+                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe,
+                    lockoutOnFailure: true);
                 if (result.Succeeded)
                 {
                     _logger.LogInformation("User logged in.");
```

**ure/Authentication/Identity/ServiceCollectionExtensions.cs**

```diff
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

### Passoword policy

- [Password](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0#password)

**パスワードポリシー**

| プロパティ             | 説明                                         | Default |
| ---------------------- | -------------------------------------------- | ------- |
| RequireDigit           | 数値[0-9]を必要とします。                    | true    |
| RequireLowercase       | 小文字[a-z]を必要とします。                  | true    |
| RequireUppercase       | 大文字[A-Z]を必要とします。                  | true    |
| RequireNonAlphanumeric | 英数字以外の文字を必要とします。             | true    |
| RequiredLength         | 最低限の長さ。                               | 6       |
| RequiredUniqueChars    | 個別の文字の数を必要とします。<br>※0000 防止 | 1       |

```diff
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<IdentityDataContext>();

+        services.Configure<IdentityOptions>(options =>
+        {
+            // Password settings.
+            options.Password.RequireDigit = false;
+            options.Password.RequireUppercase = false;
+            options.Password.RequireLowercase = false;
+            options.Password.RequireNonAlphanumeric = false;
+            options.Password.RequiredLength = 4;
+            options.Password.RequiredUniqueChars = 2;
+        });
+
```

> Don't forget the attribute of model.

**/Areas/Identity/Pages/Account/Register.cshtml.cs**

```diff
@@ -84,7 +84,7 @@ public class InputModel
             ///     directly from your code. This API may change or be removed in future releases.
             /// </summary>
             [Required]
-            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
+            [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
             [DataType(DataType.Password)]
             [Display(Name = "Password")]
             public string Password { get; set; }

```

**/Areas/Identity/Pages/Account/ResetPassword.cshtml.cs**

```diff
@@ -74,7 +74,7 @@ public class InputModel
             ///     directly from your code. This API may change or be removed in future releases.
             /// </summary>
             [Required]
-            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
+            [StringLength(100, ErrorMessage = "The {0} must be at max {1} characters long.")]
             [DataType(DataType.Password)]
             [Display(Name = "Password")]
             public string Password { get; set; }

```

### User

- [User](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0#user)

```diff
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<IdentityDataContext>();

+        services.Configure<IdentityOptions>(options =>
+        {
+            // User settings.
+            // options.User.AllowedUserNameCharacters =
+            //     "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
+            // options.User.RequireUniqueEmail = false;
+        });
+
```

### Cookie

- [Cookie の設定](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0#cookie-settings)

```diff
        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<IdentityDataContext>();

+        // Cookie settings.
+        services.ConfigureApplicationCookie(options =>
+        {
+            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
+            options.Cookie.Name = "YourAppCookieName";
+            options.Cookie.HttpOnly = true;
+            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
+            options.LoginPath = "/Identity/Account/Login";
+            // ReturnUrlParameter requires
+            //
using Microsoft.AspNetCore.Authentication.Cookies;

+            options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
+            options.SlidingExpiration = true;
         });
```


### Localization of validation messages

- [ASP.NET Core で Identity のバリデーションメッセージを日本語化する - shuhelohelo’s blog](https://shuhelohelo.hatenablog.com/entry/2019/08/22/201322)
- [IdentityErrorDescriber クラス - Microsoft Docs](https://docs.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.identity.identityerrordescriber?view=aspnetcore-6.0)

```diff
         services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
             .AddEntityFrameworkStores<IdentityDataContext>()
+            .AddErrorDescriber<JapaneseErrorDescriber>();
```

