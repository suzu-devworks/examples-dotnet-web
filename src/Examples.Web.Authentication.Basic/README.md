# Examples.Web.Authentication.Basic

## References

- [RFC 7617(ja)](https://tex2e.github.io/rfc-translater/html/rfc7617.html)

## Use AspNetCore.Authentication.Basic

- [AspNetCore.Authentication.Basic ...](https://github.com/mihirdilip/aspnetcore-authentication-basic)

Add to bootstrap code in `Program.cs`:

```cs
    //# Add Basic Authentication.
    builder.Services.AddAuthentication(defaultScheme: BasicDefaults.AuthenticationScheme)
        .AddBasicWithAspNetCore(option => builder.Configuration.GetSection("Authentication").Bind(option));
```

For usage of AspNetCore.Authentication.Basic, see Extension Methods.

Set the value for `BasicAuthenticationOption` in `appsettings.json`:

```json
{
  "Authentication": {
    "Realm": "localhost.local"
  }
}
```

In this sample, the account is set as a constant of InMemoryUserRepository.


## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Authentication.Basic
dotnet new webapp -o src/Examples.Web.Authentication.Basic
dotnet sln add src/Examples.Web.Authentication.Basic/
cd src/Examples.Web.Authentication.Basic
dotnet add package AspNetCore.Authentication.Basic

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```

