# ASP.NET Core NET 8.0 Tips 

## Doesn't it work with https?

```shell
dotnet run
```

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5181
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.

warn: Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware[3]
      Failed to determine the https port for redirect.
```

The only problem is the order of profiles.

```json
"profiles": {
    "http": {  // <- This is run.
      ...
    },
    "https": {
      ...
    },
    "IIS Express": {
      ...
    }
  }
```

If you change the order, it becomes the default.

```json
"profiles": {
    "https": {  // <- This is run.
      ...
    },
    "http": {
      ...
    },
    "IIS Express": {
      ...
    }
  }
```

## Worried message 2024.001

```
warn: Microsoft.AspNetCore.Hosting.Diagnostics[15]
      Overriding HTTP_PORTS '8080' and HTTPS_PORTS ''. Binding to values defined by URLS instead 'http://localhost:5181'.
```

This is because environment variables are defined within the .NET 8.0 container.

```shell
env | grep _PORTS
```

```
ASPNETCORE_HTTP_PORTS=8080
```

No need to worry. All you'll see is a warning that "will be overwritten by launchSettings."

If you are concerned about it, please delete it from the environment variables.

```shell
export ASPNETCORE_HTTP_PORTS=

or

unset ASPNETCORE_HTTP_PORTS
```