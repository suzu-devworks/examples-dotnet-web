# ASP.NET Core Identity external provider authentication

Enables users to sign in using OAuth 2.0 with credentials from external authentication providers.

## Table of contents

- [Google OAuth 2.0 login setup in ASP.NET Core](./oauth2/google_oauth2.md)
- [Microsoft Account OAuth 2.0 login setup with ASP.NET Core](./oauth2/microsoft_oauth2.md)
- [GitHub OAuth 2.0 login setup in ASP.NET Core](./oauth2/github_oauth2.md)


## References

- [Facebook, Google, and external provider authentication in ASP.NET Core - Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/social/?view=aspnetcore-8.0&tabs=visual-studio-code)


## Configuration

### Create OAuth 2.0 Client ID and secret

Social login providers assign Application Id and Application Secret tokens during the registration process. 

The Secret Manager tool hides implementation details, such as where and how the values are stored.

- [Safe storage of app secrets in development in ASP.NET Core - Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=linux#secret-manager)

```shell
dotnet user-secrets init
```

List the secrets:

```shell
dotnet user-secrets list
```

### Setup login providers required by your application

- [Google ...](./oauth2/google_oauth2.md)
- [Microsoft ...](./oauth2/microsoft_oauth2.md)
- [Github ...](./oauth2/github_oauth2.md)

- [External OAuth authentication providers - Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/social/other-logins?view=aspnetcore-8.0)


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
