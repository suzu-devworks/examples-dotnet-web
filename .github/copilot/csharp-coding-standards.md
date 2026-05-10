# C# Coding Standards

All formatting and style rules are governed by .editorconfig.
EnforceCodeStyleInBuild is enabled, so violations fail the build.

## General

- Write code, comments, and documentation in concise English.
- Use XML documentation comments on reusable public or internal APIs.

## Naming and Style

- Follow standard .NET naming conventions.
- Prefer predefined C# type keywords (for example, string) over BCL aliases.
- Do not use the this. qualifier unless required by existing code.

## Method Design

- Introduce a parameter class when a method has many parameters.
- Always suffix async methods returning Task or ValueTask with Async.
- Always add CancellationToken cancellationToken = default as the last parameter of every public async method returning Task or ValueTask.

## Test Naming

- Prefer descriptive test names in the form When_Condition_Then_ExpectedResult.
