# OAuth 2.0 provider authentication

Enables users to sign in using OAuth 2.0 with credentials from external authentication providers.

## Table of Contents <!-- omit in toc -->

- [OAuth 2.0 provider authentication](#oauth-20-provider-authentication)
  - [References](#references)
  - [Configuration](#configuration)
    - [Create OAuth 2.0 Client ID and secret](#create-oauth-20-client-id-and-secret)
    - [Setup login providers required by your application](#setup-login-providers-required-by-your-application)
    - [Multiple authentication providers](#multiple-authentication-providers)


## References

- [Facebook, Google, and external provider authentication in ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/social/?view=aspnetcore-8.0&tabs=visual-studio-code)

<!-- ----- -->

## Configuration

### Create OAuth 2.0 Client ID and secret

Social login providers assign Application Id and Application Secret tokens during the registration process. 

The Secret Manager tool hides implementation details, such as where and how the values are stored.

- [Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=linux#secret-manager)

```shell
dotnet user-secrets init
```

List the secrets:

```shell
dotnet user-secrets list
```

### Setup login providers required by your application

- [Google ...](./oauth2/oauth2_google.md)
- [Microsoft ...](./oauth2/oauth2_microsoft.md)
- [Github ...](./oauth2/oauth2_github.md)
- Other provider
    - [External OAuth authentication providers](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/social/other-logins?view=aspnetcore-8.0)


### Multiple authentication providers

When the app requires multiple providers, chain the provider extension methods from `AddAuthentication`:

```cs
    builder.Services.AddAuthentication()
        .AddGoogle(options =>
        {
            IConfigurationSection googleAuthNSection =
            config.GetSection("Authentication:Google");
            options.ClientId = googleAuthNSection["ClientId"];
            options.ClientSecret = googleAuthNSection["ClientSecret"];
        })
        .AddFacebook(options =>
        {
            IConfigurationSection FBAuthNSection =
            config.GetSection("Authentication:FB");
            options.ClientId = FBAuthNSection["ClientId"];
            options.ClientSecret = FBAuthNSection["ClientSecret"];
        })
        .AddMicrosoftAccount(microsoftOptions =>
        {
            microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
            microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
        })
        .AddTwitter(twitterOptions =>
        {
            twitterOptions.ConsumerKey = config["Authentication:Twitter:ConsumerAPIKey"];
            twitterOptions.ConsumerSecret = config["Authentication:Twitter:ConsumerSecret"];
            twitterOptions.RetrieveUserDetails = true;
        });
```
