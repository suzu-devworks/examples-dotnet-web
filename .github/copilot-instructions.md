# GitHub Copilot Instructions

## Scope

This file defines only how GitHub Copilot should work in this repository.
Project facts, architecture, conventions, and validation details are maintained
in the repository context skill.

## Repository Context Source

- `.github/skills/repository-context/SKILL.md`

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

- Use the repository context skill for verification flow and
  project-specific commands.
- For Markdown files related to GitHub Copilot or workspace skills,
  run markdownlint and keep formatting clean.
- If verification is skipped, clearly state why.
- In responses, include what changed, why, how it was verified, and remaining risks.
- Prefer official documentation when citing evidence and include primary
  source URLs when relevant.

### Commit Messages

- Follow Conventional Commits: `<type>(<scope>): <subject>`.
- Use `feat` for new features, `fix` for bug fixes, `docs` for
  documentation changes, `style` for formatting, `refactor` for code
  changes that neither fix a bug nor add a feature, `test` for adding or
  updating tests, and `chore` for maintenance tasks.
- Include a brief description of the change in the subject line.
- If the change is significant, include a more detailed description in the
  body of the commit message.
- Avoid using vague commit messages like "Update code" or "Fix bug" without context.
- If the change is a breaking change, include `BREAKING CHANGE:` in the
  commit message body and describe the impact and necessary actions for
  users.

### Repository Context Usage

- Before starting any coding task, consult the repository context skill
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

### Security and Secrets

- Never suggest or generate real credentials, secrets, or machine-
  specific values in tracked files.
- Use placeholders, `dotnet user-secrets`, or environment variables for
  sensitive configuration.
- Flag any existing secrets found in files and recommend remediation before proceeding.
