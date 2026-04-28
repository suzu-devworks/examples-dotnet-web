# Examples.Web.Authentication.Certificate

## Table of Contents <!-- omit in toc -->

- [Microsoft.AspNetCore.Authentication.Certificate](#microsoftaspnetcoreauthenticationcertificate)
  - [Set up this project](#set-up-this-project)
    - [1. Set up authentication (Program.cs)](#1-set-up-authentication-programcs)
    - [2. Set up middleware pipeline (Program.cs)](#2-set-up-middleware-pipeline-programcs)
    - [3. Set up Kestrel TLS handshake (appsettings.json)](#3-set-up-kestrel-tls-handshake-appsettingsjson)
    - [4. Set up authorization](#4-set-up-authorization)
  - [Authentication flows](#authentication-flows)
- [Scenarios](#scenarios)
  - [1. When importing a CA certificate into the OS](#1-when-importing-a-ca-certificate-into-the-os)
  - [2. When managing CA certificates in a custom store](#2-when-managing-ca-certificates-in-a-custom-store)
    - [2.1. Set up Authentication](#21-set-up-authentication)
    - [2.2. Configure custom store PATH](#22-configure-custom-store-path)
    - [2.3. Set up Kestrel TLS handshake](#23-set-up-kestrel-tls-handshake)
- [Development](#development)
  - [Build](#build)
  - [Run](#run)
  - [How the project was initialized](#how-the-project-was-initialized)
- [References](#references)

## Microsoft.AspNetCore.Authentication.Certificate

Provides classes to support certificate authentication.

When hosting with Kestrel, a certificate is requested during the handshake, so the certificate selection screen will be displayed almost constantly throughout the site.

To restrict access to only authenticated users on a page-by-page basis, use app-level authentication.

### Set up this project

#### 1. Set up authentication (Program.cs)

Add the following to `Program.cs`:

```cs
builder.Services.AddAuthentication(
        CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate();
```

#### 2. Set up middleware pipeline (Program.cs)

```cs
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
```

#### 3. Set up Kestrel TLS handshake (appsettings.json)

```json
{
  "Kestrel": {
    "Endpoints": {
      "Https": {
        "Url": "https://+:7021",
        "ClientCertificateMode": "RequireCertificate"
      }
    }
  },
}
```

#### 4. Set up authorization

Page-level authentication is done using AuthorizeAttribute, just like other authentication methods.

However, since certificate authentication requires TLS when accessing the site, it seems like it wouldn't make a difference if everything was done on a page-by-page basis.

```cs
builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build());
```

### Authentication flows

## Scenarios

### 1. When importing a CA certificate into the OS

The default certificate authentication handler is configured to refer to the OS's trusted certificate store, so it works simply by registering the CA certificate that generated the client certificate you want to verify with the OS.

Register all intermediate certificates if necessary.

```shell
sudo assets/example.ca-root.crt /usr/local/share/ca-certificates/
sudo update-ca-certificates
```

### 2. When managing CA certificates in a custom store

Using a custom trust store turned out to be much more difficult than I expected.

The official documentation suggested it could be done with `ChainTrustValidationMode` and `CustomTrustStore`, but it was rejected before authentication even began.

Furthermore, enabling revocation checks (CRL, OCSP) resulted in failure.

#### 2.1. Set up Authentication

Configure the options within `AddCertificate`:

```cs
var certCollection = CertificateLoader.LoadCertificate(
    builder.Configuration["Authentication:Certificate:CustomTrustStore"]);

builder.Services.AddAuthentication(
        CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options =>
    {
        options.ChainTrustValidationMode = X509ChainTrustMode.CustomRootTrust;
        options.CustomTrustStore.AddRange(certCollection);
        options.RevocationMode = X509RevocationMode.NoCheck;
    });
```

#### 2.2. Configure custom store PATH

This is specified in appsettings.json:

```json
{
  "Authentication": {
    "Certificate": {
      "CustomTrustStore": "../../assets/"
    }
  }
}
```

#### 2.3. Set up Kestrel TLS handshake

This setting doesn't seem possible in `appsettings.json`, so it requires directly modifying `Program.cs`:

```cs
builder.Services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.ClientCertificateMode =
            Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.RequireCertificate;

        httpsOptions.OnAuthenticate = (context, sslOptions) =>
        {
            sslOptions.CertificateChainPolicy = new X509ChainPolicy
            {
                TrustMode = X509ChainTrustMode.CustomRootTrust,
                RevocationMode = X509RevocationMode.NoCheck
            };

            sslOptions.CertificateChainPolicy.CustomTrustStore.AddRange(certCollection);
        };
    });
});
```

## Development

### Build

Build this project from the repository root:

```shell
dotnet build src/Examples.Web.Authentication.Certificate/
```

### Run

Run this project from the repository root:

```shell
dotnet run --project src/Examples.Web.Authentication.Certificate/ -lp https
```

### How the project was initialized

This project was initialized with the following commands:

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Authentication.Certificate
dotnet new webapp -o src/Examples.Web.Authentication.Certificate
dotnet sln add src/Examples.Web.Authentication.Certificate/
cd src/Examples.Web.Authentication.Certificate
dotnet add reference ../Examples.Web.Infrastructure/
dotnet add reference ../Examples.Web.Infrastructure.Assets/
dotnet add package Microsoft.AspNetCore.Authentication.Certificate

dotnet user-secrets init
cd ../../

# Check outdated packages
dotnet list package --outdated
```

## References

- [Configure certificate authentication in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/certauth)
