# Examples.Web.WebApi.Grpc

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [Examples](#examples)
  - [Logging](#logging)
  - [gRPC Json Transcoding](#grpc-json-transcoding)
  - [Use Swagger](#use-swagger)
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

### gRPC Json Transcoding

gRPC JSON transcoding is an extension for ASP.NET Core that creates RESTful JSON APIs for gRPC services

- [gRPC JSON transcoding in ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/grpc/json-transcoding)

**1. Add a package reference**:

```shell
dotnet add package Microsoft.AspNetCore.Grpc.JsonTranscoding
```

**2. In the `Program.cs` file**:

```diff
  // Add services to the container.
- builder.Services.AddGrpc();
+ builder.Services.AddGrpc().AddJsonTranscoding();

```

**3. Add property group in the project file (.csproj)**:

```diff
  <PropertyGroup>
+    <IncludeHttpRuleProtos>true</IncludeHttpRuleProtos>
  </PropertyGroup>
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

Let's try testing it with curl:

```shell
curl -v -k https://localhost:7271/v1/greeter/world
```

Returns:

```console
{"message":"Hello world"}
```

### Use Swagger

OpenAPI (Swagger) is a language-agnostic specification for describing REST APIs. gRPC JSON transcoding supports generating OpenAPI from transcoded RESTful APIs.

- [gRPC JSON transcoding documentation with Swagger / OpenAPI](https://learn.microsoft.com/ja-jp/aspnet/core/grpc/json-transcoding-openapi)

**1. Add a package reference**:

```shell
dotnet add package Microsoft.AspNetCore.Grpc.Swagger
```

**2. In the `Program.cs` file**:

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

**3. Add OpenAPI descriptions from .proto comments**:

project file (.csproj):

```diff
  <PropertyGroup>
+    <GenerateDocumentationFile>true</GenerateDocumentationFile>
  </PropertyGroup>
```

In the `Program.cs` file:

```diff
+ using Examples.Web.Infrastructure.Swagger;

  builder.Services.AddSwaggerGen(c =>
  {
      c.SwaggerDoc("v1",
          new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });


    var filePath = Path.Combine(System.AppContext.BaseDirectory, "Server.xml");
    c.IncludeXmlComments(filePath);
    c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
  });
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

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```
