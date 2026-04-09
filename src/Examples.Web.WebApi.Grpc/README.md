# Examples.Web.WebApi.Grpc

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [Examples](#examples)
  - [Logging](#logging)
  - [Routing](#routing)
    - [Auto gRPC Service registration](#auto-grpc-service-registration)
  - [Open Api](#open-api)
    - [gRPC Json Transcoding](#grpc-json-transcoding)
    - [Use OpenAPI (Swagger)](#use-openapi-swagger)
    - [Add OpenAPI descriptions from .proto comments](#add-openapi-descriptions-from-proto-comments)
  - [Fluent Validation](#fluent-validation)
- [Development](#development)
  - [Build](#build)
  - [Run](#run)
  - [How the project was initialized](#how-the-project-was-initialized)

## Overview

This project is intended for testing and learning the functionality of the ASP.NET gRPC API.

## Examples

### Logging

- [Can the console logs look a bit better?](../../docs/logging/logging_use_console.md#tiny-colored-console)
- [Aren't the logs being output twice?](../../docs/logging/logging_use_console.md#the-log-is-generated-twice)

### Routing

#### Auto gRPC Service registration

Registering each one individually is a hassle.

```cs
// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
```

Let's register it using reflection with [`Examples.Web.Infrastructure.Grpc.ApplicationBuilderExtensions`](./Infrastructure/Grpc/ApplicationBuilderExtensions.cs)

```diff
  // Configure the HTTP request pipeline.
- app.MapGrpcService<GreeterService>();
+ app.MapGrpcServices<Program>();
```

### Open Api

#### gRPC Json Transcoding

gRPC JSON transcoding is an extension for ASP.NET Core that creates RESTful JSON APIs for gRPC services

- [gRPC JSON transcoding in ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/grpc/json-transcoding)

**1. Add a package reference**:

```shell
dotnet add package Microsoft.AspNetCore.Grpc.JsonTranscoding
```

**2. Register in server startup code (`Program.cs`)**:

```diff
  // Add services to the container.
- builder.Services.AddGrpc();
+ builder.Services.AddGrpc().AddJsonTranscoding();

```

**3. Enable HTTP bindings and routes (google.api.http) in the project file (.csproj)**:

```diff
  <PropertyGroup>
+    <IncludeHttpRuleProtos>true</IncludeHttpRuleProtos>
  </PropertyGroup>
```

If you don't enter the above, the next message will be "File not found."

```proto
import "google/api/annotations.proto";
```

**4. Annotate gRPC methods in your .proto files with HTTP bindings and routes**:

```diff
+ import "google/api/annotations.proto";

  // The greeting service definition.
  service Greeter {
-   rpc SayHello (HelloRequest) returns (HelloReply);
+   rpc SayHello (HelloRequest) returns (HelloReply) {
+     option (google.api.http) = {
+       get: "/v1/greeter/{name}"
+     };
    }
  }
```

**5. Let's try testing it with curl**:

```shell
curl -v -k https://localhost:7271/v1/greeter/world
```

Returns:

```console
{"message":"Hello world"}
```

#### Use OpenAPI (Swagger)

> [!NOTE]
> It appears to be a transitional period.
> It seems they are changing from `Swashbuckle.AspNetCore` to `Microsoft.AspNetCore.OpenApi`.
> However, at this time, it was not possible to achieve the same behavior with `Microsoft.AspNetCore.OpenApi`.

OpenAPI (Swagger) is a language-agnostic specification for describing REST APIs. gRPC JSON transcoding supports generating OpenAPI from transcoded RESTful APIs.

- [gRPC JSON transcoding documentation with Swagger / OpenAPI](https://learn.microsoft.com/ja-jp/aspnet/core/grpc/json-transcoding-openapi)

**1. Add a package reference**:

```shell
dotnet add package Microsoft.AspNetCore.Grpc.Swagger
```

**2. Register in server startup code (`Program.cs`)**:

```diff
  var builder = WebApplication.CreateBuilder(args);
  builder.Services.AddGrpc().AddJsonTranscoding();
+ builder.Services.AddGrpcSwagger();
+ builder.Services.AddSwaggerGen(c =>
+ {
+     c.SwaggerDoc("v1",
+         new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
+ });
 
  var app = builder.Build();
+ app.UseSwagger();
+ if (app.Environment.IsDevelopment())
+ {
+     app.UseSwaggerUI(c =>
+     {
+         c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
+     });
+ }
  app.MapGrpcService<GreeterService>();
```

#### Add OpenAPI descriptions from .proto comments

**1. Enable the XML documentation file in the project file (.csproj)**:

project file (.csproj):

```diff
  <PropertyGroup>
+    <GenerateDocumentationFile>true</GenerateDocumentationFile>
  </PropertyGroup>
```

**2. Register in server startup code (`Program.cs`)**:

We'll take a shortcut by using [`Examples.Web.Infrastructure.OpenApi.SwaggerGenOptionsExtensions`](./Infrastructure/OpenApi/SwaggerGenOptionsExtensions.cs).

```diff
  builder.Services.AddSwaggerGen(c =>
  {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
+     c.IncludeXmlComments<Program>();
  });
```

### Fluent Validation

FluentValidation is a .NET library for building strict type validation rules, and is particularly useful with gRPC.

- [FluentValidation &mdash; FluentValidation  documentation](https://docs.fluentvalidation.net/en/latest/#)

**1. Add a package reference**:

```shell
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions
```

**2. Register in server startup code (`Program.cs`)**:

```diff
  var builder = WebApplication.CreateBuilder(args);

- builder.Services.AddGrpc().AddJsonTranscoding();
+ builder.Services.AddGrpc(options =>
+ {
+     options.Interceptors.Add<RequestValidationInterceptor>(); 
+ 
+ }).AddJsonTranscoding();
+ builder.Services.AddValidatorsFromAssemblyContaining<Program>();

  var app = builder.Build();
```

[`Examples.Web.Infrastructure.Validators.RequestValidationInterceptor`](./Infrastructure/Validators/RequestValidationInterceptor.cs) is an inspector that retrieves and validates registered validators before executing a grpc method.

**3. Create validator**:

All that's left is to place the validator wherever you want.

```cs
public class UserValidator : AbstractValidator<User>
{
  public UserValidator()
  {
    RuleFor(x => x.Name).NotNull();
  }
}
```

## Development

### Build

Build this project from the repository root:

```shell
dotnet build src/Examples.Web.WebApi.Grpc/Examples.Web.WebApi.Grpc.csproj
```

### Run

Run this project from the repository root:

```shell
dotnet run --project src/Examples.Web.WebApi.Grpc/Examples.Web.WebApi.Grpc.csproj
```

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Infrastructure
dotnet new classlib -o src/Examples.Web.Infrastructure
dotnet sln add src/Examples.Web.Infrastructure/
cd src/Examples.Web.Infrastructure
cd ../../

## Examples.Web.Infrastructure.Swagger
dotnet new classlib -o src/Examples.Web.Infrastructure.Swagger
dotnet sln add src/Examples.Web.Infrastructure.Swagger/
cd src/Examples.Web.Infrastructure.Swagger
dotnet add package Swashbuckle.AspNetCore
dotnet add package Swashbuckle.AspNetCore.Annotations
cd ../../

## Examples.Web.Infrastructure.GrpcClient
dotnet new classlib -o src/Examples.Web.Infrastructure.GrpcClient
dotnet sln add src/Examples.Web.Infrastructure.GrpcClient/
cd src/Examples.Web.Infrastructure.GrpcClient
dotnet add package Grpc.Net.Client
dotnet add package Google.Protobuf
dotnet add package Grpc.Tools
cd ../../

## Examples.Web.WebApi.Grpc
dotnet new webapp -o src/Examples.Web.WebApi.Grpc
dotnet sln add src/Examples.Web.WebApi.Grpc/
cd src/Examples.Web.WebApi.Grpc
dotnet add reference ../Examples.Web.Infrastructure
dotnet add reference ../Examples.Web.Infrastructure.Swagger
dotnet add reference ../Examples.Web.Infrastructure.GrpcClient
dotnet add package Microsoft.AspNetCore.Grpc.JsonTranscoding
dotnet add package Microsoft.AspNetCore.Grpc.Swagger
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```
