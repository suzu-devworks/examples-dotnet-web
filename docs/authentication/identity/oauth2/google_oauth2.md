# Google OAuth 2.0 login setup in ASP.NET Core

## Reference

- [Google external login setup in ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-8.0)


## Create the Google OAuth 2.0 Client ID and secret

- [Google Cloud Platform](https://console.cloud.google.com/)

Save the Client ID and Client Secret for use in the app's configuration.

## Store the Google client ID and secret

```shell
dotnet user-secrets set "Authentication:Google:ClientId" "<client-id>"
dotnet user-secrets set "Authentication:Google:ClientSecret" "<client-secret>"
```

List the secrets:

```shell
dotnet user-secrets list
```

## Configure Google authentication

```shell
dotnet add package Microsoft.AspNetCore.Authentication.Google
```

```diff
--- a/src/Examples.Web.Authentication.Identity/Program.cs
+++ b/src/Examples.Web.Authentication.Identity/Program.cs
@@ -11,6 +11,13 @@
             ?? throw new InvalidOperationException("Connection string 'IdentityDataContextConnection' not found.");
     });
 
+builder.Services.AddAuthentication()
+    .AddGoogle(googleOptions =>
+    {
+        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
+        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
+    });
+
 builder.Services.AddControllersWithViews();
 
 var app = builder.Build();
```
