# Examples.Web.Authentication.Oidc

## Table of Contents <!-- omit in toc -->

- [Microsoft.AspNetCore.Authentication.OpenIdConnect](#microsoftaspnetcoreauthenticationopenidconnect)
  - [Setup OIDC Authentication Server (Identity Provider)](#setup-oidc-authentication-server-identity-provider)
    - [Case of Microsoft Entra ID](#case-of-microsoft-entra-id)
    - [Case of Auth0](#case-of-auth0)
  - [Setup this project](#setup-this-project)
    - [1. Set up authentication (Program.cs)](#1-set-up-authentication-programcs)
    - [2. Setup middleware pipeline (Program.cs)](#2-setup-middleware-pipeline-programcs)
    - [3. Setup Authorization (Program.cs)](#3-setup-authorization-programcs)
    - [4. Create `Logout.cshtml`](#4-create-logoutcshtml)
    - [5. Create `SignedOut.cshtml`](#5-create-signedoutcshtml)
    - [6. (Optional) Create `Login.cshtml`](#6-optional-create-logincshtml)
    - [7. Create `_LoginPartial.cshtml`](#7-create-_loginpartialcshtml)
  - [Setup secrets](#setup-secrets)
- [Development](#development)
  - [Build](#build)
  - [Run](#run)
  - [How the project was initialized](#how-the-project-was-initialized)
- [References](#references)

## Microsoft.AspNetCore.Authentication.OpenIdConnect

Contains types that enable support for OpenIdConnect based authentication.

### Setup OIDC Authentication Server (Identity Provider)

There are several options for OIDC authentication servers (Identity Providers) that can be used without the hassle of environment setup. Here are some examples:

Using an external cloud service (SaaS):

- [Microsoft Entra ID](https://azure.microsoft.com/ja-jp/get-started/azure-portal)
- [Auth0](https://auth0.com/jp)

Using a local solution:

- [Keycloak](https://www.keycloak.org/)

#### Case of Microsoft Entra ID

I gave up because it required entering my credit card number.

#### Case of Auth0

1. Create an account on the Auth0 official website (you can create one instantly using your Google/GitHub account, etc.).
2. From the side menu, select Applications > Create Application.
3. Select Name: MyTestApp, etc. / Type: Regular Web Applications and click Create.
4. Configure the following in the Settings tab and save:
    - Allowed Callback URLs: `https://localhost:{user port}/signin-oidc`
    - Allowed Logout URLs: `https://localhost:{user port}/signout-callback-oidc`
5. Note down the following three items on the same screen:
    - Domain
    - Client ID
    - Client Secret

**Optional**:

- **If you want to hide the sign-up**: *Authentication* > *Database* > `Username-Password-Authentication` > *settings* > *Disable Sign Ups*

- **If you want to use the role in your application**: *Actions* > *Triggers* > `Post Login` > *Add Action(Create Custom Action)*

```js
  const namespace = 'https://my-app.example.com'; // Any identifier (URL format recommended)
  if (event.authorization) {
    // Add roles to ID tokens.
    api.idToken.setCustomClaim(`${namespace}/roles`, event.authorization.roles);
    // Also add roles to access tokens (when using API integration).
    api.accessToken.setCustomClaim(`${namespace}/roles`, event.authorization.roles);
  }
```

- **To use Pushed Authorization Requests (PAR)**: *Settings* > *Advanced* > *Settings* > *Allow Pushed Authorization Requests (PAR)*

### Setup this project

#### 1. Set up authentication (Program.cs)

```cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(options =>
{
    builder.Configuration.GetSection("OpenIDConnectSettings").Bind(options);

    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.ResponseType = OpenIdConnectResponseType.Code;

    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;

    options.MapInboundClaims = false;
    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
    options.TokenValidationParameters.RoleClaimType = "https://my-app.example.com/roles";

    // .NET 9 feature
    options.PushedAuthorizationBehavior = PushedAuthorizationBehavior.Require;
});
```

If you are using a role, please match the name of the claim you added in Auth0 with `options.TokenValidationParameters.RoleClaimType`.

#### 2. Setup middleware pipeline (Program.cs)

The authentication and authorization middleware must be placed after routing:

```cs
app.UseRouting();

app.UseAuthentication();   // Authentication middleware (required)
app.UseAuthorization();    // Authorization middleware
```

#### 3. Setup Authorization (Program.cs)

A better approach is to force authorization for the whole app and opt out for unsecure pages:

```cs
var requireAuthPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(requireAuthPolicy);
```

Don't forget to add AllowAnonymous to MapStaticAssets.

```cs
app.MapStaticAssets()
    .AllowAnonymous();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
```

All you need to do is add the `AllowAnonymousAttribute` to the necessary parts.

#### 4. Create `Logout.cshtml`

```cs
[Authorize]
public class LogoutModel : PageModel
{
    public IActionResult OnGetAsync()
    {
        return SignOut(new AuthenticationProperties
        {
            RedirectUri = "/Account/SignedOut"
        },
        // Clear auth cookie
        CookieAuthenticationDefaults.AuthenticationScheme,
        // Redirect to OIDC provider signout endpoint
        OpenIdConnectDefaults.AuthenticationScheme);
    }
}
```

#### 5. Create `SignedOut.cshtml`

```cs
[AllowAnonymous]
public class SignedOutModel : PageModel
{
    public void OnGet()
    {
    }
}
```

#### 6. (Optional) Create `Login.cshtml`

```cs
[AllowAnonymous]
public class LoginModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public async Task OnGetAsync()
    {
        var properties = GetAuthProperties(ReturnUrl);
        await HttpContext.ChallengeAsync(properties);
    }

    private static AuthenticationProperties GetAuthProperties(string? returnUrl)
    {
        const string pathBase = "/";

        // Prevent open redirects.
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = pathBase;
        }
        else if (!Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
        {
            returnUrl = new Uri(returnUrl, UriKind.Absolute).PathAndQuery;
        }
        else if (returnUrl[0] != '/')
        {
            returnUrl = $"{pathBase}{returnUrl}";
        }

        return new AuthenticationProperties { RedirectUri = returnUrl };
    }
}
```

#### 7. Create `_LoginPartial.cshtml`

```razor
<ul class="navbar-nav gap-2">
    @if (Context.User.Identity!.IsAuthenticated)
    {
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-page="/Account/Logout">Logout</a>
        </li>

        <span class="nav-link text-dark">Hi @Context.User.Identity.Name</span>
    }
    else
    {
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-page="/Index">Login</a>
        </li>
    }
</ul>
```

### Setup secrets

Set the value you noted down in Auth0:

```shell
dotnet user-secrets set "OpenIDConnectSettings:Authority" "https://{Domain}"
dotnet user-secrets set "OpenIDConnectSettings:ClientId" "{Client ID}"
dotnet user-secrets set "OpenIDConnectSettings:ClientSecret" "{Client Secret}" 
```

## Development

### Build

Build this project from the repository root:

```shell
dotnet build src/Examples.Web.Authentication.Oidc/
```

### Run

Run this project from the repository root:

```shell
dotnet run --project src/Examples.Web.Authentication.Oidc/ -lp https
```

### How the project was initialized

This project was initialized with the following commands:

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Authentication.Oidc
dotnet new webapp -o src/Examples.Web.Authentication.Oidc
dotnet sln add src/Examples.Web.Authentication.Oidc/
cd src/Examples.Web.Authentication.Oidc
dotnet add reference ../Examples.Web.Infrastructure/
dotnet add package Microsoft.AspNetCore.Authentication.OpenIdConnect

dotnet user-secrets init
cd ../../

# Check outdated packages
dotnet list package --outdated
```

## References

- [Configure OpenID Connect Web (UI) authentication in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/configure-oidc-web-authentication)
