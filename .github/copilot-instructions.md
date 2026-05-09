# GitHub Copilot Instructions

## Scope

This file defines only how GitHub Copilot should work in this repository.
Project facts, architecture, conventions, and validation details are maintained
in the skills listed below.

## Repository Context Source

- `.github/skills/repository-context/SKILL.md`
- `.github/skills/csharp-coding-standards/SKILL.md`
- `.github/skills/commit-convention/SKILL.md`
- `.github/skills/security-secrets/SKILL.md`

## How to Work

### Language and Communication

- Think and reason in English.
- Write code, comments, and documentation in concise English.
- Respond to the user in Japanese in chat.
- Use Japanese for all user-facing explanations, including reviews,
  summaries, and progress reports.
- Do not change locale segments in reference URLs
  (for example `ja-jp`) unless explicitly requested.

### Change Strategy

- Keep diffs focused, minimal, and easy to review.
- Avoid broad style-only refactors unless explicitly requested.
- Prefer simple, reversible implementations.
- Preserve existing conventions unless the user asks to change policy.
- Ask for confirmation before destructive or high-impact changes.

### Validation and Reporting

- Use the repository-context skill for verification flow and
  project-specific commands.
- For Markdown files related to GitHub Copilot or workspace skills,
  run markdownlint and keep formatting clean.
- If verification is skipped, clearly state why.
- In responses, include what changed, why, how it was verified, and remaining risks.
- Prefer official documentation when citing evidence and include primary
  source URLs when relevant.

### Repository Context Usage

- Before starting any coding task, consult the relevant skills
  for project conventions and the task completion checklist.
- Derive current branch, git status, and live workspace facts from the
  current workspace context instead of cached snapshots.

### Tool Usage Priority

- **Symbol-level operations** (find definition, rename, replace body):
  prefer workspace-native symbol-aware tools when available over broad
  manual rewrites.
- **Exploration and context gathering**: use built-in tools
  (`read_file`, `grep_search`, `file_search`, `semantic_search`) because
  they are faster and do not modify the workspace.
- **Terminal commands**: use only when built-in tools are clearly
  insufficient, such as running `dotnet build`, `dotnet test`, or
  scripts. Prefer targeted commands over broad shell pipelines.
- **File edits**: use `replace_string_in_file` or
  `multi_replace_string_in_file` for targeted edits and avoid rewriting
  entire files unless necessary.
- **Destructive or irreversible actions** (delete files, force push,
  drop tables): always ask for user confirmation first.

### MCP Usage

- **Microsoft products** (Azure, .NET, Microsoft 365, Power Platform, etc.):
  always use the Microsoft Learn MCP server (`microsoftdocs/mcp`) to
  retrieve the latest official documentation before answering. Do not rely
  solely on training data. Include the Microsoft Learn document URL(s)
  consulted in every response.
- **Library and framework specifications or setup**: always use the
  Context7 MCP server (`io.github.upstash/context7`) to fetch the latest
  official documentation. First resolve the library ID, then fetch the
  library docs. Prioritize information obtained via Context7 over training
  data, which may be outdated.
