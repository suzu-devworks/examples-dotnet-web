# Examples.Web.WebApi.Grpc

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [Examples](#examples)
  - [gRPC Json Transcoding](#grpc-json-transcoding)
  - [Use Swagger](#use-swagger)
- [Development](#development)
  - [Build](#build)
  - [Run](#run)
  - [How the project was initialized](#how-the-project-was-initialized)

## Overview

This project is intended for testing and learning the functionality of the ASP.NET gRPC API.

## Examples

### gRPC Json Transcoding

gRPC JSON transcoding is an extension for ASP.NET Core that creates RESTful JSON APIs for gRPC services

- [gRPC JSON transcoding in ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/grpc/json-transcoding)

### Use Swagger

OpenAPI (Swagger) is a language-agnostic specification for describing REST APIs. gRPC JSON transcoding supports generating OpenAPI from transcoded RESTful APIs.

- [gRPC JSON transcoding documentation with Swagger / OpenAPI](https://learn.microsoft.com/ja-jp/aspnet/core/grpc/json-transcoding-openapi)

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

## Examples.Web.Infrastructure.GrpcClient
dotnet new classlib -o src/Examples.Web.Infrastructure.GrpcClient
dotnet sln add src/Examples.Web.Infrastructure.GrpcClient/
cd src/Examples.Web.Infrastructure.GrpcClient
cd ../../../

## Examples.Web.WebApi.Grpc
dotnet new webapp -o src/Examples.Web.WebApi.Grpc
dotnet sln add src/Examples.Web.WebApi.Grpc/
cd src/Examples.Web.WebApi.Grpc
dotnet add reference ../Examples.Web.Infrastructure
dotnet add reference ../Examples.Web.Infrastructure.GrpcClient

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```
