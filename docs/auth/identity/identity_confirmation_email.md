# Account confirmation and password recovery

## Table of Contents <!-- omit in toc -->

- [Account confirmation and password recovery](#account-confirmation-and-password-recovery)
  - [References](#references)
  - [Account confirmation and password recovery in ASP.NET Core](#account-confirmation-and-password-recovery-in-aspnet-core)
    - [Implement IEmailSender](#implement-iemailsender)
    - [Configure app to support email](#configure-app-to-support-email)
    - [Disable default account verification when Account.RegisterConfirmation has been scaffolded](#disable-default-account-verification-when-accountregisterconfirmation-has-been-scaffolded)
    - [Change email and activity timeout](#change-email-and-activity-timeout)
    - [Change all data protection token lifespans](#change-all-data-protection-token-lifespans)


## References

- [Account confirmation and password recovery in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/accconfirm?view=aspnetcore-8.0&tabs=netcore-cli)


## Account confirmation and password recovery in ASP.NET Core

### Implement IEmailSender

We recommend using [SendGrid](https://sendgrid.kke.co.jp/) or another email service to send email rather than SMTP. SMTP is difficult to secure and set up correctly.

The sample outputs the contents of the email to a file and confirms it.

[see FakeEmailSender.cs ...](/src/Examples.Web.Authentication.Identity/Services/FakeEmailSender.cs)

`confirm.html` will be created in the `temp` folder, so please view it in your browser and authenticate.
For Dev Containers, it is convenient to use the [Live Preview](https://marketplace.visualstudio.com/items?itemName=ms-vscode.live-server) extension


### Configure app to support email

Add EmailSender as a transient service:

```diff
--- a/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
+++ b/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
@@ -1,9 +1,11 @@
 using System;
 using Microsoft.AspNetCore.Authentication.Cookies;
 using Microsoft.AspNetCore.Identity;
+using Microsoft.AspNetCore.Identity.UI.Services;
 using Microsoft.EntityFrameworkCore;
 using Microsoft.Extensions.DependencyInjection;
 using Examples.Web.Authentication.Identity.Areas.Identity.Data;
+using Examples.Web.Authentication.Identity.Services;
 
 namespace Examples.Web.Infrastructure.Authentication.Identity;
 
@@ -79,6 +81,8 @@ public static class ServiceCollectionExtensions
             options.SlidingExpiration = true;
         });
 
+        services.AddTransient<IEmailSender, FakeEmailSender>();
+
         return services;
     }
```


### Disable default account verification when Account.RegisterConfirmation has been scaffolded

The user is redirected to the Account.RegisterConfirmation where they can select a link to have the account confirmed. 
The default Account.RegisterConfirmation is used only for testing, automatic account verification should be disabled in a production app.

```diff
--- a/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/RegisterConfirmation.cshtml.cs
+++ b/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/RegisterConfirmation.cshtml.cs
@@ -60,7 +60,7 @@ public async Task<IActionResult> OnGetAsync(string email, string returnUrl = nul
 
             Email = email;
             // Once you add a real email sender, you should remove this code that lets you confirm the account
-            DisplayConfirmAccountLink = true;
+            DisplayConfirmAccountLink = false;
             if (DisplayConfirmAccountLink)
             {
                 var userId = await _userManager.GetUserIdAsync(user);
```

### Change email and activity timeout

The default inactivity timeout is 14 days.

```diff
--- a/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
+++ b/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
@@ -83,6 +83,12 @@ public static class ServiceCollectionExtensions
 
         services.AddTransient<IEmailSender, FakeEmailSender>();
 
+        services.ConfigureApplicationCookie(o =>
+        {
+            o.ExpireTimeSpan = TimeSpan.FromDays(5);
+            o.SlidingExpiration = true;
+        });
+
         return services;
     }
```

### Change all data protection token lifespans

The built in Identity user tokens have a 1 day timeout.

```diff
--- a/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
+++ b/src/Examples.Web.Authentication.Identity/Infrastructure/Authentication/Identity/ServiceCollectionExtensions.cs
@@ -89,6 +89,9 @@ public static class ServiceCollectionExtensions
             o.SlidingExpiration = true;
         });
 
+        services.Configure<DataProtectionTokenProviderOptions>(o =>
+            o.TokenLifespan = TimeSpan.FromHours(3));
+
         return services;
     }
```
