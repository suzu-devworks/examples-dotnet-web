# GitHub OAuth 2.0 login setup in ASP.NET Core

## Table of Contents <!-- omit in toc -->

- [GitHub OAuth 2.0 login setup in ASP.NET Core](#github-oauth-20-login-setup-in-aspnet-core)
  - [Reference](#reference)
  - [Create the OAuth 2.0 Client ID and secret](#create-the-oauth-20-client-id-and-secret)
    - [Store the client ID and secret](#store-the-client-id-and-secret)
  - [Configure Google authentication](#configure-google-authentication)

## Reference

- [OAuth app authorization](https://docs.github.com/ja/apps/oauth-apps/building-oauth-apps/authorizing-oauth-apps)
- [AspNet.Security.OAuth.Providers](https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers)

## Create the OAuth 2.0 Client ID and secret

- [Register a new OAuth application - GitHub](https://github.com/settings/applications/new/)

1. Settings / Developer Settings / OAuth Apps / New OAuth App
2. Register a new OAuth application
     - Application name: any<br />So that you don't have trouble later.
     - Homepage URL: any<br />Enable URL format check.
     - Authorization callback URL: `https://localhost:{PORT}/signin-github`
     - Enable Device Flow: `false`<br />Authorize users for headless applications such as CLI tools and Git credential managers.
3. Register application
4. Generate a new client secret
5. Save the Client ID and Client Secret for use in the app's configuration.


### Store the client ID and secret

```shell
dotnet user-secrets set "Authentication:Github:ClientId" "<client-id>"
dotnet user-secrets set "Authentication:Github:ClientSecret" "<client-secret>"
```

List the secrets:

```shell
dotnet user-secrets list
```

## Configure Google authentication

```shell
dotnet add package AspNet.Security.OAuth.GitHub

# GitHub API Client Library
# dotnet add package OctoKit
```

```diff
--- a/src/Examples.Web.Authentication.Identity/Program.cs
+++ b/src/Examples.Web.Authentication.Identity/Program.cs
@@ -21,8 +21,16 @@
     {
         microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"]!;
         microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]!;
+    })
+    .AddGitHub(githubOptions =>
+    {
+        githubOptions.ClientId = builder.Configuration["Authentication:Github:ClientId"]!;
+        githubOptions.ClientSecret = builder.Configuration["Authentication:Github:ClientSecret"]!;
+        // https://docs.github.com/en/developers/apps/building-oauth-apps/scopes-for-oauth-apps
+        githubOptions.Scope.Add("user:email");
     });
```
