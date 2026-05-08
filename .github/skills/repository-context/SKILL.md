---
name: repository-context
description: "Use when: you need examples-dotnet-web project structure, tech stack, build or test commands, or workspace-specific development context."
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

Projects are placed directly under `src/`. Auxiliary libraries and tools
are placed in separate subdirectories under `src/` as needed.

## Key Configuration

- src/Directory.Build.props enables TreatWarningsAsErrors and
  EnforceCodeStyleInBuild
- src/Directory.Build.targets contains shared clean targets
- global.json configures Microsoft.Testing.Platform
- nuget.config defines package sources
- .editorconfig defines formatting and naming rules

## Build and Validation Expectations

- TreatWarningsAsErrors is enabled, so builds must be warning-free
- EnforceCodeStyleInBuild is enabled, so style issues fail the build
- GenerateDocumentationFile is enabled; if you add reusable APIs,
  keep XML docs in place
- Tests should be deterministic and avoid machine-specific dependencies

## Project Conventions

- Reuse shared extensions from Examples.Web.Infrastructure when possible.
- Preserve middleware ordering in Program.cs.
- Prefer configuration binding over hardcoded values.
- Keep environment-specific behavior explicit.

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
  tracked files (see security-secrets skill).
4. If public or reusable APIs changed, ensure XML documentation remains
  appropriate (see csharp-coding-standards skill).
5. Use Conventional Commits format when preparing a commit
  (see commit-convention skill).

## Workspace Facts

- Default branch: main
- Current working branch and git status should be taken from live
  workspace context, not cached files.
- Available build task: dotnet: build
