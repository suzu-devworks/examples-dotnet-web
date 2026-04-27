# Examples.Web.Infrastructure.OpenApi

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [How to use](#how-to-use)
  - [To add security requirements](#to-add-security-requirements)
- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Overview

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

## Development

### How the project was initialized

This project was initialized with the following commands:

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Infrastructure.OpenApi
dotnet new classlib -o src/Examples.Web.Infrastructure.OpenApi
dotnet sln add src/Examples.Web.Infrastructure.OpenApi/
cd src/Examples.Web.Infrastructure.OpenApi
dotnet add package Microsoft.AspNetCore.OpenApi

cd ../../

# Check outdated packages
dotnet list package --outdated
```
