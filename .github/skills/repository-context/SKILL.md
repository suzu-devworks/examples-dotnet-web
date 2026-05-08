---
name: repository-context
description: "Use when: you need examples-dotnet-web project structure, coding conventions, validation steps, build or test commands, or workspace-specific development context."
---

# Repository Context

## Purpose

This repository is a personal learning and experimentation workspace for
ASP.NET web development.
It covers authentication, API design, middleware, routing, Blazor,
gRPC, OpenAPI, and logging.

## Tech Stack

- Language: C#
- Platform: .NET 10.0 for apps, with shared libraries targeting net8.0
  and net10.0
- Frameworks: Minimal API, MVC, Razor Pages, Blazor, gRPC,
  ASP.NET Core Identity
- Test runner: Microsoft.Testing.Platform with xUnit v3
- Supporting tools: NLog, Swashbuckle, Dev Containers, GitHub Actions

## Repository Structure

The monorepo lives under src/ and includes these primary projects:

- Examples.Web.Authentication.Basic
- Examples.Web.Authentication.Certificate
- Examples.Web.Authentication.Cookie
- Examples.Web.Authentication.Identity
- Examples.Web.Authentication.JwtBearer
- Examples.Web.Authentication.Oidc
- Examples.Web.Blazor.WebApp
- Examples.Web.Infrastructure
- Examples.Web.Infrastructure.Assets
- Examples.Web.Infrastructure.GrpcClient
- Examples.Web.Infrastructure.OpenApi
- Examples.Web.Infrastructure.Swagger
- Examples.Web.Infrastructure.Tests
- Examples.Web.WebApi
- Examples.Web.WebApi.Grpc
- Examples.Web.WebApp
- fixtures/

## Key Configuration

- src/Directory.Build.props enables TreatWarningsAsErrors and
  EnforceCodeStyleInBuild
- src/Directory.Build.targets contains shared clean targets
- global.json configures Microsoft.Testing.Platform
- nuget.config defines package sources
- .editorconfig defines formatting and naming rules

## Code Style and Conventions

- Write code, comments, and documentation in concise English
- Use XML documentation comments on reusable public or internal APIs
- Use 4 spaces for C# and 2 spaces for XML, JSON, YAML, and Markdown
- Sort System namespaces first and avoid separate using groups
- Follow standard .NET naming conventions
- Prefer predefined C# type keywords such as string over BCL aliases
  such as String
- Do not use the this. qualifier unless required by existing code
- Preserve middleware ordering in Program.cs
- Prefer configuration binding over hardcoded values
- Keep environment-specific behavior explicit
- Reuse shared extensions from Examples.Web.Infrastructure when possible
- Never commit real secrets; use placeholders, dotnet user-secrets,
  or environment variables

## Build and Validation Expectations

- TreatWarningsAsErrors is enabled, so builds must be warning-free
- EnforceCodeStyleInBuild is enabled, so style issues fail the build
- GenerateDocumentationFile is enabled; if you add reusable APIs,
  keep XML docs in place
- Tests should be deterministic and avoid machine-specific dependencies
- Prefer descriptive test names in the form
  When_Condition_Then_ExpectedResult

## Commands

Run these from the repository root:

```bash
dotnet tool restore
dotnet restore examples-dotnet-web.slnx
dotnet build examples-dotnet-web.slnx
dotnet test examples-dotnet-web.slnx
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

## Task Completion Checklist

1. Run dotnet build examples-dotnet-web.slnx and ensure it succeeds with
  zero warnings.
2. Run dotnet test examples-dotnet-web.slnx and ensure all tests pass.
3. Confirm no real credentials or machine-specific values were added to
  tracked files.
4. If public or reusable APIs changed, ensure XML documentation remains
  appropriate.
5. Use Conventional Commits with the format
  `<type>(<scope>): <subject>` when preparing a commit.

## Workspace Facts

- Default branch: main
- Current working branch and git status should be taken from live
  workspace context, not cached files.
- Available build task: dotnet: build
