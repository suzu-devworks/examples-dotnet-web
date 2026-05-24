# Tests and Test Assets

## Test Style and Naming

- Test code should prioritize readability and behavior verification for sample scenarios.
- Place test code and test projects under `src/` (for example `src/Examples.Web.Infrastructure.Tests`).
- Use xUnit conventions (`[Fact]`/`[Theory]`) unless a project explicitly uses another framework.
- Method names should be descriptive and clearly communicate the scenario.
- Keep tests deterministic and avoid dependency on external services unless intentionally testing integration points.

Examples:

- `When_HeaderIsMissing_Then_ReturnsUnauthorized`
- `When_ValidTokenIsProvided_Then_ReturnsSuccess`

## Test Assets and Sensitive Files

- Never commit production secrets, real credentials, or machine-specific certificate material.
- For local/dev certificate files, use repository scripts when generation is required (for example
  `scripts/openssl-generate-certs.sh` and `.devcontainer/ssl/ssl-cert-generate.sh`).
- Tests and sample code should read physical files from the path specified by `TEST_ASSETS_PATH`.
- CI sets `TEST_ASSETS_PATH` to `${{ github.workspace }}/assets`; keep local execution compatible with this pattern.

## Environment-Dependent Tests and Assumptions

- Some tests are intended to run only when required assets or environment settings are available.
- Unless explicitly requested, do not remove or rewrite existing environment-dependent skip logic.
- Follow repository patterns for fixture and environment checks used in tests.
- When touching auth or hosting behavior, prefer adding focused tests over broad end-to-end rewrites.
