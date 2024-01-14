# Debug

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
