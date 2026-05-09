# Validation and Commit

## Validation Checklist

- Run dotnet build and ensure zero warnings.
- Run dotnet test and ensure all tests pass.
- For Markdown files related to Copilot or workspace settings,
  run markdownlint and keep formatting clean.
- If verification is skipped, state why.
- In reports, include what changed, why, how it was verified,
  and remaining risks.

## Commit Convention

Follow Conventional Commits:

type(scope): subject

Types:

- feat: new feature
- fix: bug fix
- docs: documentation only
- style: formatting, no logic change
- refactor: neither a bug fix nor a new feature
- test: adding or updating tests
- chore: maintenance tasks

Rules:

- Include a brief description in the subject line.
- Add a body for significant changes.
- Avoid vague messages without context.
- For breaking changes, add BREAKING CHANGE: in the body with impact
  and migration steps.
