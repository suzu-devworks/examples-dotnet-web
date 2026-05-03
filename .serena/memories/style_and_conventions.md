# Code Style and Conventions

## Language

- C# with modern features for the targeted .NET version
- XML documentation comments on public/internal reusable APIs

## Formatting (.editorconfig)

- Indentation: 4 spaces for C# (.cs), 2 for XML/JSON/YAML/Markdown
- Charset: UTF-8 (project files: UTF-8 BOM, CRLF; others: LF)
- `insert_final_newline = true` for C#
- `trim_trailing_whitespace = true`
- Sort `System.*` usings first; no separate import groups

## Naming

- Standard .NET conventions (PascalCase for types/methods, camelCase for locals)
- `this.` qualifier: not used (silent)
- Predefined type keywords preferred over BCL type names (e.g., `string` over `String`)

## Build Constraints

- `TreatWarningsAsErrors = true` — no warnings allowed
- `EnforceCodeStyleInBuild = true` — style is checked at build time
- `GenerateDocumentationFile = true` — XML docs generated (1591 suppressed)

## Commit Conventions (Conventional Commits)

Format: `<type>(<scope>): <subject>`
Types: feat, fix, docs, style, refactor, test, chore

## ASP.NET Conventions

- Preserve middleware ordering in Program.cs
- Prefer configuration binding over hardcoded values
- Keep environment-specific behavior explicit (Development vs non-development)
- Reuse shared extensions from Examples.Web.Infrastructure
- Never commit real secrets; use placeholders + user-secrets or env vars

## Testing

- Descriptive test names: `When_Condition_Then_ExpectedResult`
- Deterministic tests; minimize machine-specific dependencies
- Test runner: Microsoft.Testing.Platform (configured in global.json)

## gRPC

- Preserve proto build properties and import behavior
- Keep proto sharing patterns between server and client intact
