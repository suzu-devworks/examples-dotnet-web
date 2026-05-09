---
name: commit-convention
description: "Use when: you need to write a commit message, review commit message format, or follow the Conventional Commits standard for this repository."
---

# Commit Message Convention

Follow [Conventional Commits](https://www.conventionalcommits.org/):
`<type>(<scope>): <subject>`.

## Types

- `feat`: new feature
- `fix`: bug fix
- `docs`: documentation only
- `style`: formatting, no logic change
- `refactor`: code change that neither fixes a bug nor adds a feature
- `test`: adding or updating tests
- `chore`: maintenance tasks

## Rules

- Include a brief description in the subject line.
- For significant changes, add a body with more detail.
- Avoid vague messages like "Update code" or "Fix bug" without context.
- For breaking changes, add `BREAKING CHANGE:` in the body and describe
  impact and migration steps.
