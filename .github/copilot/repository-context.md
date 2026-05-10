# Repository Context

## Purpose

This repository is a personal learning workspace for ASP.NET web development. It covers authentication, API design, middleware, routing, Blazor, gRPC, OpenAPI, and logging.

## Tech Stack

- Language: C#
- Platform: .NET 10.0 for apps; shared libraries target net8.0 and net10.0
- Frameworks: Minimal API, MVC, Razor Pages, Blazor, gRPC, ASP.NET Core Identity
- Test runner: Microsoft.Testing.Platform with xUnit v3
- Supporting tools: NLog, Swashbuckle, Dev Containers, GitHub Actions

## Key Configuration

- src/Directory.Build.props enables TreatWarningsAsErrors and EnforceCodeStyleInBuild
- src/Directory.Build.targets contains shared clean targets
- global.json configures Microsoft.Testing.Platform
- nuget.config defines package sources
- .editorconfig defines formatting and naming rules

## Project Conventions

- Reuse shared extensions from Examples.Web.Infrastructure when possible.
- Preserve middleware ordering in Program.cs.
- Prefer configuration binding over hardcoded values.
- Keep environment-specific behavior explicit.
- Projects are placed directly under src/.
- Default branch: main.

## Commands

Run from repository root:

```bash
dotnet tool restore
dotnet restore
dotnet build
dotnet test
```

Run a specific project:

```bash
cd src/Examples.Web.WebApi
dotnet run
```

Clean generated outputs:

```bash
dotnet msbuild -t:RemoveDirectories
```
