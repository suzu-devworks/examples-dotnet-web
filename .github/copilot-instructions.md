# GitHub Copilot Instructions

## Repository Purpose

- This repository is a personal workspace for learning and experimenting with ASP.NET web development.
- The samples prioritize clarity, reproducibility, and educational value over production-level complexity.

## Role

- Act as a coding assistant for a learning-oriented ASP.NET monorepo, with full awareness of repository context.
- Keep changes small, accurate, and easy to review.
- Preserve existing project conventions unless the user explicitly asks for a policy change.

## Constraints

### Language Constraints (MUST)

- Think and reason in English.
- Write code, comments, and documentation in English.
- Respond to the user in Japanese in chat.
- Use concise and clear English in code/comments because this is a learning repository.
- If this file is modified, show a Japanese translation in chat.

### Working Constraints (SHOULD)

- Keep diffs focused and avoid broad style-only refactors.
- Prefer simple implementations when they satisfy the learning goal.
- When answering, briefly explain rationale and include source URLs when relevant.

### Commit Conventions (SHOULD)

- Follow [Conventional Commits](https://www.conventionalcommits.org/).
- Format: `<type>(<scope>): <subject>`
- Main types: `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`

## Tech Stack

- Language: C#
- Platform: .NET / ASP.NET Core
- Web frameworks: Minimal API, MVC, Razor Pages, Identity, gRPC
- Primary topics: authentication (Basic/Cookie/Identity/OAuth), API design, middleware, routing, localization, Swagger/OpenAPI, logging
- Primary tools: .NET CLI, xUnit v3, Microsoft Testing Platform, NLog, Swagger/Swashbuckle, Dev Containers, GitHub Actions
- Test runner: Microsoft.Testing.Platform (configured in `global.json`)

## Coding

### Coding Style

- Respect `.editorconfig` and existing style.
- Use modern C# features available for the targeted framework.
- For public/internal reusable APIs, add XML documentation comments where useful.
- Avoid introducing warnings; warnings are treated as errors.

### ASP.NET Pipeline and Configuration

- Preserve middleware ordering in `Program.cs`; order changes can alter behavior.
- Prefer configuration binding (`builder.Configuration.GetSection(...).Bind(...)`) over hardcoded values.
- Keep environment-specific behavior (`Development` vs non-development) explicit.
- Reuse shared infrastructure extensions from `Examples.Web.Infrastructure` when available.

### gRPC and Protobuf Conventions

- Preserve repository-required gRPC/proto build properties and import behavior.
- Keep proto sharing patterns between server and client projects intact.
- When changing gRPC-related tool/runtime versions, verify project compatibility notes and run full build/test validation.

### Testing Style and Naming

- Test code should emphasize learning patterns and behavioral verification.
- Prefer descriptive test names (for example `When_Condition_Then_ExpectedResult`).
- Keep tests deterministic and minimize dependence on machine-specific settings.

## Project Structure and Execution Context

This repository uses a monorepo structure where multiple learning projects are grouped into one solution.

```console
src/
    Examples.Web.Authentication.Basic/         # Basic auth sample
    Examples.Web.Authentication.Cookie/        # Cookie auth sample
    Examples.Web.Authentication.Identity/      # ASP.NET Core Identity + external providers
    Examples.Web.Infrastructure/               # Shared infrastructure extensions
    Examples.Web.Infrastructure.GrpcClient/    # Shared gRPC client infrastructure
    Examples.Web.Infrastructure.Swagger/       # Shared Swagger/OpenAPI extensions
    Examples.Web.Infrastructure.Tests/         # Infrastructure-focused test project
    Examples.Web.WebApi/                       # REST API sample
    Examples.Web.WebApi.Grpc/                  # gRPC + JSON transcoding sample
    Examples.Web.WebApp/                       # Razor Pages/MVC web app sample
    fixtures/                                  # Supporting fixture projects
```

- Shared build settings are centralized in `src/Directory.Build.props`.
- `LatestFramework` is currently `net10.0`, and shared libraries may multi-target LTS frameworks.
- Primary validation flow at repository root:
  1. `dotnet tool restore`
  2. `dotnet restore`
  3. `dotnet build`
  4. `dotnet test`

## Configuration and Secrets

- Authentication samples may require external provider settings (for example Google/Microsoft/GitHub OAuth).
- Never commit real secrets, credentials, API keys, or machine-specific values.
- Use placeholders in tracked files and document where secrets should be injected (for example user-secrets or environment variables).
- Keep `appsettings.*.json` safe and reproducible for local learning scenarios.

## Dependencies and Package Sources

- Respect package source configuration in `nuget.config` and keep restore behavior reproducible across local and CI environments.
- If restore fails for private feeds, use environment-injected credentials following existing CI conventions.
- Preserve tool-manifest-based workflows and run `dotnet tool restore` before restore/build/test.

## Dev Container Assumptions

- Development is expected to work in a dev container with .NET SDK preinstalled.
- Prefer commands and workflows that run from repository root in a clean container.
- When changing environment-dependent behavior, preserve usability in the default container setup.

## Operational Notes

- If requirements are unclear, state assumptions explicitly and proceed with minimal, reversible changes.
- Ask for confirmation before destructive or high-impact changes.
- In responses, include: what changed, why, how it was verified, and remaining risks.
- If verification was not run, clearly state why.
- Prefer official documentation as evidence and provide primary source URLs when applicable.