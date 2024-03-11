# Debug Tips

## Table of Contents <!-- omit in toc -->

- [Debug Tips](#debug-tips)
  - [Doesn't it work with https?](#doesnt-it-work-with-https)

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

use option `-lp, --launch-profile <launch-profile>`

```shell
dotnet run -lp https
```

```json
"profiles": {
    "http": { 
      ...
    },
    "https": { // <- This is run.
      ...
    },
    "IIS Express": {
      ...
    }
  }
```
