# Repository Context

## Purpose

This repository is a personal workspace for learning ASP.NET Core features and implementing sample applications.
It focuses on understanding how web features work through hands-on implementations for authentication,
Web API, gRPC, Razor/Blazor, OpenAPI/Swagger, hosting behavior, filters, and logging.

## Tech Stack and Setup

- .NET SDK (projects target `net10.0`, with shared framework settings in `src/Directory.Build.props`)
- ASP.NET Core and related web stacks (Razor Pages, MVC, Web API, Blazor, gRPC)
- Shared infrastructure projects under `src/Examples.Web.Infrastructure*`
- Test project: `src/Examples.Web.Infrastructure.Tests`

See [README](../../README.md) for repository purpose and devcontainer setup notes.

## Key Configuration

- `src/Directory.Build.props` enables TreatWarningsAsErrors and shared build props.
- `global.json` configures the .NET SDK and test runner where applicable.
- `nuget.config` defines package sources.
- `.editorconfig` defines formatting and naming rules; CI enforces style.
- Runtime assets used by samples/tests should be loaded from the path provided by the
  `TEST_ASSETS_PATH` environment variable (CI uses `${{ github.workspace }}/assets`).

## Project Conventions

- Place projects under `src/` with clear naming (`Examples.Web.*`).
- Keep sample implementations focused and educational over over-engineered abstractions.
- Reuse shared behaviors through `Examples.Web.Infrastructure*` projects when appropriate.
- Place design documents, explanations, and other supporting materials under `docs/` in an appropriate subfolder.
- Keep authentication and hosting samples isolated by project so each scenario can be studied independently.
- Environment-dependent tests should use runtime checks and skip when prerequisites are missing.

## Build and Test Commands

Common local/CI flow:

```bash
dotnet tool restore
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test --configuration Release --no-build --verbosity normal
```

Clean generated outputs with:

```bash
dotnet msbuild -t:RemoveDirectories
```

## Default branch

Default branch: `main`
