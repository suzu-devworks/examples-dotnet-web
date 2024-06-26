# Dev Containers .NET 8.0 container tips.

## Worried message 

```
warn: Microsoft.AspNetCore.Hosting.Diagnostics[15]
      Overriding HTTP_PORTS '8080' and HTTPS_PORTS ''. Binding to values defined by URLS instead 'http://localhost:5181'.
```

This is because `ASPNETCORE_HTTP_PORTS=8080` variables are defined within the .NET 8.0 container.

- [Dockerfile](https://github.com/dotnet/dotnet-docker/blob/main/src/runtime-deps/8.0/bookworm-slim/amd64/Dockerfile#L7)


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