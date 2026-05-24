# Validation and Commit

## Validation Checklist

- Run `dotnet tool restore` when tools are required by the workflow.
- Run `dotnet restore` before build unless a prior step already restored packages.
- Run `dotnet build --configuration Release --no-restore` and ensure zero warnings/errors.
- Run `dotnet test --configuration Release --no-build --verbosity normal` and ensure tests pass
  (or clearly document skipped tests and why).
- Ensure `TEST_ASSETS_PATH` is set when tests/samples depend on repository assets.
- For documentation changes related to Copilot or workspace settings, keep Markdown formatting clean.
- If verification is skipped, state the reason and any known risks.
- In PR descriptions, include what changed, why, how it was verified, and remaining risks.

## Scenario Checks (When Relevant)

- For authentication or hosting sample changes, run a minimal local smoke check of the affected app.
- For changes in shared infrastructure projects, verify at least one dependent sample project builds/runs as expected.

## Commit Convention

Follow Conventional Commits:

```text
type(scope): subject
```

Common types:

- `feat`: new feature
- `fix`: bug fix
- `docs`: documentation only
- `style`: formatting, no logic change
- `refactor`: neither bug fix nor new feature
- `test`: adding or updating tests
- `chore`: maintenance tasks

Rules:

- Include a brief description in the subject line.
- Add a body for significant changes explaining rationale and verification steps.
- For breaking changes, include `BREAKING CHANGE:` in the body with impact and migration steps.
