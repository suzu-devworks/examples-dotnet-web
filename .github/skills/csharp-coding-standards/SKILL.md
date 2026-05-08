---
name: csharp-coding-standards
description: "Use when: you need C# coding standards, naming rules, formatting conventions, or .NET-specific coding guidelines for this repository."
---

# C# Coding Standards

All formatting and style rules are governed by `.editorconfig` at the repository
root. Always follow the settings defined there — `EnforceCodeStyleInBuild` is
enabled, so violations fail the build.

## General

- Write code, comments, and documentation in concise English.
- Use XML documentation comments on reusable public or internal APIs.

## Naming

- Follow standard .NET naming conventions.
- Prefer predefined C# type keywords such as `string` over BCL aliases
  such as `String`.
- Do not use the `this.` qualifier unless required by existing code.

## Method Design

- Introduce a parameter class when a method has many parameters.
- Always suffix async methods returning `Task` or `ValueTask` with `Async`.
- Always add `CancellationToken cancellationToken = default` as the last
  parameter of every public async method returning `Task` or `ValueTask`.

## Test Naming

- Prefer descriptive test names in the form
  `When_Condition_Then_ExpectedResult`.
