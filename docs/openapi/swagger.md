# Swagger


## Troubleshooting

### `does not contain an entry point.

This error has been observed to occur whenever Swashbuckle.AspnetCore is added to a clslib project.

- https://github.com/dotnet/aspnetcore/issues/14370

```console
error : Assembly '/workspaces/examples-dotnet-web/src/Examples.Web.Infrastructure/bin/Debug/net7.0/Examples.Web.Infrastructure.dll' does not contain an entry point. 

exited with code 3. [/workspaces/examples-dotnet-web/src/Examples.Web.Infrastructure/Examples.Web.Infrastructure.csproj::TargetFramework=net7.0]

```

Configure the project to not generate Swagger documents.

```xml
  <PropertyGroup>
    <OpenApiGenerateDocuments>false</OpenApiGenerateDocuments>
  </PropertyGroup>
```

## References

* [Swagger.io](https://swagger.io/)
* [Swashbuckle.AspNetCore - github](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

