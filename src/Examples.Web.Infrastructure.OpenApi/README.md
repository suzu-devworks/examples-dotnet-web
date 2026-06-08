# Examples.Web.Infrastructure.OpenApi

## Overview and Purpose

This project provides reusable OpenAPI transformers for authentication-related settings.

It currently includes:

- Document transformer for adding a security scheme (default: Bearer).
- Operation transformer for adding security requirements to endpoints that are not marked with `AllowAnonymous`.

I created it to avoid repeating similar OpenAPI security settings across sample projects.

## How to use

Add a project reference first.

```xml
<ItemGroup>
 <ProjectReference Include="..\Examples.Web.Infrastructure.OpenApi\Examples.Web.Infrastructure.OpenApi.csproj" />
</ItemGroup>
```

### To add security requirements

Register transformers in `AddOpenApi`.

```csharp
using Examples.Web.Infrastructure.OpenApi;

builder.Services.AddOpenApi(options =>
{
  // Add Bearer security scheme to the OpenAPI document.
  options.AddDocumentTransformer<SecurityRequirementDocumentTransformer>();

  // Require authentication on operations unless AllowAnonymous is specified.
  options.AddOperationTransformer<SecurityRequirementOperationTransformer>();
});
```

If you want to customize the scheme (for example, API key), use the following configuration.

```csharp
using Examples.Web.Infrastructure.OpenApi;
using Microsoft.OpenApi;

builder.Services.AddOpenApi(options =>
{
  options.AddDocumentTransformer(new SecurityRequirementDocumentTransformer(() => new OpenApiSecurityScheme
  {
    Type = SecuritySchemeType.ApiKey,
    Name = "X-API-Key",
    In = ParameterLocation.Header,
    Scheme = "ApiKey",
    Description = "Enter API key"
  }));

  options.AddOperationTransformer(new SecurityRequirementOperationTransformer("ApiKey"));
});
```
