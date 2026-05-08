---
name: security-secrets
description: "Use when: you need guidance on handling credentials, secrets, or sensitive configuration in this repository."
---

# Security and Secrets

- Never commit real credentials, secrets, or machine-specific values in
  tracked files.
- Use placeholders, `dotnet user-secrets`, or environment variables for
  sensitive configuration.
- Flag any existing secrets found in files and recommend remediation
  before proceeding.
