# GitHub Copilot Instructions

## Language and Communication

- Think and reason in English.
- Write code, comments, and documentation in concise English.
- Respond to the user in Japanese in chat.
- Use Japanese for all user-facing explanations, including reviews, summaries, and progress reports.
- Do not change locale segments in reference URLs (for example `ja-jp`) unless explicitly requested.

---

## Security and Secrets

- Never commit real credentials, secrets, or machine-specific values in tracked files.
- Use placeholders, `dotnet user-secrets`, or environment variables for sensitive configuration.
- Flag any existing secrets found in files and recommend remediation before proceeding.

---

## Change Strategy

- Keep diffs focused, minimal, and easy to review.
- Avoid broad style-only refactors unless explicitly requested.
- Prefer simple, reversible implementations.
- Preserve existing conventions unless the user asks to change policy.
- Ask for confirmation before destructive or high-impact changes.
- Ask for confirmation before adding/upgrading dependencies.
- Ask for confirmation before changing public API behavior.

## Priority Order

When instructions conflict, follow this order:

1. System/developer instructions.
2. This repository instruction file.
3. User request.
4. Existing codebase conventions.

---

## Tool Usage

- **Symbol-level operations** (find definition, rename, replace body): prefer workspace-native symbol-aware tools over broad manual rewrites.
- **Exploration and context gathering**: use VS Code workspace capabilities for file discovery, text search, and semantic lookup.
- **Terminal**: use only when built-in tools are insufficient, such as running `dotnet build`, `dotnet test`, or scripts.
- **Model/agent-specific names**: prefer capability-based wording; when explicit names are necessary, use VS Code-native names available in this environment.
- **File edits**: prefer targeted edits using VS Code editing capabilities, and avoid rewriting entire files unless necessary.
- **Destructive or irreversible actions** (delete files, force push, drop tables): always ask for user confirmation first.

## MCP Usage

- **Microsoft products** (Azure, .NET, Microsoft 365, etc.): use official Microsoft Learn documentation retrieval capabilities for the latest guidance; include consulted URL(s) in responses.
- **Library and framework specifications**: use up-to-date library documentation retrieval capabilities. If server IDs are required by the active environment, use configured VS Code MCP server IDs and resolve the library ID before fetching docs.

---

## Split References

Read only when needed:

- Before implementation in an unfamiliar area:
  [`.github/copilot/repository-context.md`](./copilot/repository-context.md)
- Before modifying C# code or tests:
  [`.github/copilot/csharp-coding-standards.md`](./copilot/csharp-coding-standards.md)
- Before final verification or commit preparation:
  [`.github/copilot/validation-and-commit.md`](./copilot/validation-and-commit.md)

### Update Rule

- Keep this file minimal and always-on.
- Move details to split files unless they are required in most tasks.
- Promote a split rule into this file only if it is repeatedly needed.
