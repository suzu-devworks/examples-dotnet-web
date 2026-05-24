# C# Coding Standards

All formatting and style rules are governed by `.editorconfig` and the repository's build-time enforcement.
`EnforceCodeStyleInBuild` is enabled in shared props so violations can fail the build.

## General

- Write code, comments, and documentation in concise English.
- Use XML documentation comments (`<summary>`, `<param>`, `<returns>`) on reusable public or internal APIs.
- Keep sample code easy to read and educational; avoid unnecessary abstraction.

## Naming and Style

- Follow standard .NET naming conventions.
- Prefer predefined C# type keywords (for example, `string`) over BCL aliases.
- Avoid `this.` qualifier unless necessary for clarity.
- Keep namespaces and folder structure aligned with project boundaries (`Examples.Web.*`).

## Method Design

- Introduce a parameter class when a method has many parameters.
- Suffix async methods returning `Task`/`ValueTask` with `Async`.
- Add `CancellationToken cancellationToken = default` as the last parameter of public async
  methods returning `Task` or `ValueTask` where appropriate.

## ASP.NET-Specific Guidelines

- Keep `Program.cs` and startup wiring focused; extract repeated setup into extension methods when needed.
- Use options binding/validation for configuration-heavy features.
- For authentication and middleware samples, prefer explicit registration order and clear comments when order matters.
- Preserve public endpoint behavior unless change is intentional and documented.

## Test Naming

- Prefer descriptive test names in the form `When_Condition_Then_ExpectedResult`.

## Test Style and Naming

Test guidance (naming, asset handling, and environment assumptions) has been moved to
[`tests-and-assets.md`](./tests-and-assets.md).
