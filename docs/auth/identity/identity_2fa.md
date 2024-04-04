# Two factor authentication 

## Table of Contents <!-- omit in toc -->

- [Two factor authentication](#two-factor-authentication)
  - [References](#references)
  - [Enable QR code generation](#enable-qr-code-generation)
    - [Download qrcode.js library.](#download-qrcodejs-library)
    - [Scaffold Identity to generate](#scaffold-identity-to-generate)
    - [Modify codes](#modify-codes)
  - [Change the site name in the QR code](#change-the-site-name-in-the-qr-code)


## References

- [Enable QR code generation for TOTP authenticator apps in ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/identity-enable-qrcodes?view=aspnetcore-8.0)


## Enable QR code generation

### Download qrcode.js library.

- [qrcode.js](https://davidshimjs.github.io/qrcodejs/

1. download .zip 
2. copy `qrcode.min.js` to `wwwroot\lib` 


### Scaffold Identity to generate

Check the specification method:

```shell
dotnet aspnet-codegenerator identity --listFiles
```

Do Scaffold:

```shell
dotnet aspnet-codegenerator identity -dc Examples.Web.Authentication.Identity.Areas.Identity.Data.IdentityDataContext --files 'Account.Manage.EnableAuthenticator' --databaseProvider 'sqlite'
```

Was it necessary to specify everything?

```shell
dotnet aspnet-codegenerator identity -dc Examples.Web.Authentication.Identity.Areas.Identity.Data.IdentityDataContext --files 'Account.Register;Account.Login;Account.Logout;Account.RegisterConfirmation;Account.ResetPassword;Account.Manage.EnableAuthenticator' --databaseProvider 'sqlite'
```

### Modify codes

```diff
--- /dev/null
+++ b/src/Examples.Web.Authentication.Identity/wwwroot/js/qr.js
@@ -0,0 +1,9 @@
+window.addEventListener("load", () => {
+  const uri = document.getElementById("qrCodeData").getAttribute('data-url');
+  new QRCode(document.getElementById("qrCode"),
+    {
+      text: uri,
+      width: 150,
+      height: 150
+    });
+});
```

```diff
--- a/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/Manage/EnableAuthenticator.cshtml
+++ b/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/Manage/EnableAuthenticator.cshtml
@@ -56,4 +56,7 @@ <h3>@ViewData["Title"]</h3>
 
 @section Scripts {
     <partial name="_ValidationScriptsPartial" />
+
+    <script type="text/javascript" src="~/lib/qrcode.min.js"></script>
+    <script type="text/javascript" src="~/js/qr.js"></script>
 }
```

## Change the site name in the QR code

```diff
--- a/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/Manage/EnableAuthenticator.cshtml.cs
+++ b/src/Examples.Web.Authentication.Identity/Areas/Identity/Pages/Account/Manage/EnableAuthenticator.cshtml.cs
@@ -180,7 +180,8 @@ private string GenerateQrCodeUri(string email, string unformattedKey)
             return string.Format(
                 CultureInfo.InvariantCulture,
                 AuthenticatorUriFormat,
-                _urlEncoder.Encode("Microsoft.AspNetCore.Identity.UI"),
+                // it must always be URL encoded.
+                _urlEncoder.Encode("Examples.Web.Authentication.Identity"),
                 _urlEncoder.Encode(email),
                 unformattedKey);
         }
```
