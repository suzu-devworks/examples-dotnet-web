# Suggested Commands

## Primary Workflow (run from repository root)

```bash
dotnet tool restore       # Restore .NET tools
dotnet restore            # Restore NuGet packages
dotnet build              # Build all projects
dotnet test               # Run all tests
```

## Individual Project

```bash
cd src/Examples.Web.WebApi
dotnet run                # Run a specific project
```

## Clean

```bash
dotnet msbuild -t:RemoveDirectories   # Remove bin/ and obj/
```

## Linting / Code Style

- Enforced at build time: `EnforceCodeStyleInBuild=true`
- Warnings treated as errors: `TreatWarningsAsErrors=true`
- No separate lint command needed; `dotnet build` catches style issues

## Certificate setup (Dev Container)

```bash
./.devcontainer/ssl/ssl_cert_generate.sh
```
