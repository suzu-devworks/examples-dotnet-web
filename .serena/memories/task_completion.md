# Task Completion Checklist

When completing a coding task in this repository:

1. **Build**: `dotnet build examples-dotnet-web.slnx` — must succeed with zero warnings (warnings are errors)
2. **Test**: `dotnet test examples-dotnet-web.slnx` — all tests must pass
3. **Style**: automatically enforced by build (EnforceCodeStyleInBuild=true)
4. **Secrets**: verify no real credentials or machine-specific values in tracked files
5. **Commit message**: follow Conventional Commits format `<type>(<scope>): <subject>`
6. **Documentation**: if public/internal APIs were added/changed, ensure XML doc comments are present

## Pre-commit Steps

```bash
dotnet tool restore
dotnet restore examples-dotnet-web.slnx
dotnet build examples-dotnet-web.slnx
dotnet test examples-dotnet-web.slnx
```
