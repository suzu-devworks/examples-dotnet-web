# GitHub Copilot Instructions

## Language and Communication

- Think and reason in English.
- Write code, comments, and documentation in concise English.
- Respond to the user in Japanese in chat.
- Use Japanese for all user-facing explanations, including reviews,
  summaries, and progress reports.
- Do not change locale segments in reference URLs
  (for example `ja-jp`) unless explicitly requested.

---

## Security and Secrets

- Never commit real credentials, secrets, or machine-specific values
  in tracked files.
- Use placeholders, `dotnet user-secrets`, or environment variables
  for sensitive configuration.
- Flag any existing secrets found in files and recommend remediation
  before proceeding.

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

- **Symbol-level operations** (find definition, rename, replace body):
  prefer workspace-native symbol-aware tools over broad manual rewrites.
- **Exploration and context gathering**: use built-in tools
  (`read_file`, `grep_search`, `file_search`, `semantic_search`).
- **Terminal**: use only when built-in tools are insufficient, such as
  running `dotnet build`, `dotnet test`, or scripts.
- **File edits**: use `apply_patch` for targeted edits and avoid
  rewriting entire files unless necessary.
- **Destructive or irreversible actions** (delete files, force push,
  drop tables): always ask for user confirmation first.

## MCP Usage

- **Microsoft products** (Azure, .NET, Microsoft 365, etc.):
  use the Microsoft Learn MCP server (`microsoftdocs/mcp`) for the
  latest official documentation. Include the URL(s) consulted in responses.
- **Library and framework specifications**: use the Context7 MCP server
  (`io.github.upstash/context7`) to fetch the latest docs.
  Resolve the library ID first, then fetch docs.

---

## Split References

Read only when needed:

- Before implementation in an unfamiliar area:
  `.github/copilot/guide/repository-context.md`
- Before modifying C# code or tests:
  `.github/copilot/guide/csharp-coding-standards.md`
- Before final verification or commit preparation:
  `.github/copilot/guide/validation-and-commit.md`

### Update Rule

- Keep this file minimal and always-on.
- Move details to split files unless they are required in most tasks.
- Promote a split rule into this file only if it is repeatedly needed.
