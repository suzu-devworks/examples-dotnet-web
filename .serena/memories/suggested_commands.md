# Suggested Commands

## Primary Workflow (run from repository root)

```bash
dotnet tool restore                     # Restore .NET tools
dotnet restore examples-dotnet-web.slnx # Restore NuGet packages
dotnet build examples-dotnet-web.slnx   # Build all projects
dotnet test examples-dotnet-web.slnx    # Run all tests
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
./.devcontainer/ssl/ssl-cert-generate.sh
```
