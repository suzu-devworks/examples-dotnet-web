# ASP.NET Core Identity Customization

## References

- [Identity model customization in ASP.NET Core - Microsoft Learn](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-8.0
)

## Naming Login

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


## Localization of validation messages

Create a class that implements `IdentityErrorDescriber` and set it with `AddErrorDescriber()`.

- [IdentityErrorDescriber クラス - Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.identity.identityerrordescriber?view=aspnetcore-8.0)
- [JapaneseErrorDescriber.cs ...](/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/JapaneseErrorDescriber.cs)


```diff
--- a/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
+++ b/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
@@ -26,7 +26,8 @@ public static class ServiceCollectionExtensions
             .UseSqlite(configureOption.ConnectionString));
 
         services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
-            .AddEntityFrameworkStores<IdentityDataContext>();
+            .AddEntityFrameworkStores<IdentityDataContext>()
+            .AddErrorDescriber<JapaneseErrorDescriber>();
 
         services.Configure<IdentityOptions>(options =>
         {
```
