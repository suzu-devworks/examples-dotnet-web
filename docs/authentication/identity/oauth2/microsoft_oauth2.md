# Microsoft Account OAuth 2.0 login setup with ASP.NET Core

## Reference

- [Microsoft Account external login setup with ASP.NET Core - Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-8.0)


## Create client secret

- [App registrations - Azure portal](https://go.microsoft.com/fwlink/?linkid=2083908)


## Store the Microsoft client ID and secret

```shell
dotnet user-secrets set "Authentication:Microsoft:ClientId" "<client-id>"
dotnet user-secrets set "Authentication:Microsoft:ClientSecret" "<client-secret>"
```

List the secrets:

```shell
dotnet user-secrets list
```

## Configure Google authentication

```shell
dotnet add package Microsoft.AspNetCore.Authentication.MicrosoftAccount
```

```diff
--- a/src/Examples.Web.Authentication.Identity/Program.cs
+++ b/src/Examples.Web.Authentication.Identity/Program.cs
@@ -16,6 +16,11 @@
     {
         googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
         googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
+    })
+    .AddMicrosoftAccount(microsoftOptions =>
+    {
+        microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"]!;
+        microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]!;
     });
```
