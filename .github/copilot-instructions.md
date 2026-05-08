# GitHub Copilot Instructions

## Scope

This file defines only how GitHub Copilot should work in this repository.
Project facts, architecture, conventions, and validation details are maintained in Serena memories.

## Serena References (Source of Truth for What/Why)

- `.serena/memories/project_overview.md`
- `.serena/memories/style_and_conventions.md`
- `.serena/memories/suggested_commands.md`
- `.serena/memories/task_completion.md`
- `.serena/memories/workspace_state.md` for generated session-start workspace facts

## How to Work

### Language and Communication

- Think and reason in English.
- Write code, comments, and documentation in concise English.
- Respond to the user in Japanese in chat.
- Use Japanese for all user-facing explanations, including reviews, summaries, and progress reports.
- Do not change locale segments in reference URLs (for example `ja-jp`) unless explicitly requested.

### Change Strategy

- Keep diffs focused, minimal, and easy to review.
- Avoid broad style-only refactors unless explicitly requested.
- Prefer simple, reversible implementations.
- Preserve existing conventions unless the user asks to change policy.
- Ask for confirmation before destructive or high-impact changes.

### Validation and Reporting

- Use the Serena task completion checklist and suggested command flow for verification.
- For Markdown files related to GitHub Copilot or Serena, run markdownlint and keep formatting clean.
- If verification is skipped, clearly state why.
- In responses, include what changed, why, how it was verified, and remaining risks.
- Prefer official documentation when citing evidence and include primary source URLs when relevant.

### Commit Messages

- Follow Conventional Commits: `<type>(<scope>): <subject>`.
- Use `feat` for new features, `fix` for bug fixes, `docs` for documentation changes, `style` for formatting, `refactor` for code changes that neither fix a bug nor add a feature, `test` for adding or updating tests, and `chore` for maintenance tasks.
- Include a brief description of the change in the subject line.
- If the change is significant, include a more detailed description in the body of the commit message
- Avoid using vague commit messages like "Update code" or "Fix bug" without context.
- If the change is a breaking change, include `BREAKING CHANGE:` in the commit message body and describe the impact and necessary actions for users.

### Serena Usage

- Before starting any coding task, consult Serena memories for project conventions and the task completion checklist.
- Use `.serena/memories/workspace_state.md` when current branch, git status, or workspace session facts matter.

### Tool Usage Priority

- **Symbol-level operations** (find definition, rename, replace body): prefer Serena MCP tools (`find_symbol`, `rename_symbol`, `replace_symbol_body`, etc.) over manual file edits.
- **Exploration and context gathering**: use built-in tools (`read_file`, `grep_search`, `file_search`, `semantic_search`) — these are faster and do not modify the workspace.
- **Terminal commands**: use only when built-in tools are clearly insufficient (e.g., running `dotnet build`, `dotnet test`, or scripts). Prefer targeted commands over broad shell pipelines.
- **File edits**: use `replace_string_in_file` or `multi_replace_string_in_file` for targeted edits; avoid rewriting entire files unless necessary.
- **Destructive or irreversible actions** (delete files, force push, drop tables): always ask for user confirmation first.

### Security and Secrets

- Never suggest or generate real credentials, secrets, or machine-specific values in tracked files.
- Use placeholders, `dotnet user-secrets`, or environment variables for sensitive configuration.
- Flag any existing secrets found in files and recommend remediation before proceeding.
