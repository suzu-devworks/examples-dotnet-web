# Project Overview: examples-dotnet-web

## Purpose

Personal learning and experimentation workspace for ASP.NET web development.
Covers authentication, API design, middleware, routing, and more.

## Tech Stack

- Language: C#
- Platform: .NET 10.0 (LatestFramework), multi-targets net8.0 and net10.0 for shared libs
- Frameworks: Minimal API, MVC, Razor Pages, Blazor, gRPC, ASP.NET Core Identity
- Primary topics: authentication (Basic/Cookie/Identity/OAuth/JWT/OIDC/Certificate), OpenAPI/Swagger, logging, gRPC
- Test runner: Microsoft.Testing.Platform (xUnit v3)
- Tools: NLog, Swashbuckle, Dev Containers, GitHub Actions

## Project Structure (monorepo under src/)

- Examples.Web.Authentication.Basic/
- Examples.Web.Authentication.Certificate/
- Examples.Web.Authentication.Cookie/
- Examples.Web.Authentication.Identity/
- Examples.Web.Authentication.JwtBearer/
- Examples.Web.Authentication.Oidc/
- Examples.Web.Blazor.WebApp/
- Examples.Web.Infrastructure/           # Shared infrastructure extensions
- Examples.Web.Infrastructure.Assets/
- Examples.Web.Infrastructure.GrpcClient/
- Examples.Web.Infrastructure.OpenApi/
- Examples.Web.Infrastructure.Swagger/
- Examples.Web.Infrastructure.Tests/
- Examples.Web.WebApi/                   # REST API sample
- Examples.Web.WebApi.Grpc/              # gRPC + JSON transcoding sample
- Examples.Web.WebApp/                   # Razor Pages/MVC web app sample
- fixtures/                              # Supporting fixture projects

## Key Config Files

- src/Directory.Build.props: shared build settings (TreatWarningsAsErrors=true, EnforceCodeStyleInBuild=true)
- src/Directory.Build.targets: clean targets
- global.json: test runner = Microsoft.Testing.Platform
- nuget.config: package source configuration
- .editorconfig: code style rules
