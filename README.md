# examples-dotnet-web

![Dynamic XML Badge](https://img.shields.io/badge/dynamic/xml?url=https%3A%2F%2Fraw.githubusercontent.com%2Fsuzu-devworks%2Fexamples-dotnet-web%2Frefs%2Fheads%2Fmain%2Fsrc%2FDirectory.Build.props&query=%2F%2FLatestFramework&logo=dotnet&label=Framework&color=%23512bd4)
[![build](https://github.com/suzu-devworks/examples-dotnet-web/actions/workflows/dotnet-build.yml/badge.svg)](https://github.com/suzu-devworks/examples-dotnet-web/actions/workflows/dotnet-build.yml)
[![CodeQL](https://github.com/suzu-devworks/examples-dotnet-web/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/suzu-devworks/examples-dotnet-web/actions/workflows/github-code-scanning/codeql)

## What is the purpose of this repository?

This repository is just my personal playground for learning ASP.NET web programming.

The content here might actually be helpful to other developers facing similar issues.

However, please keep in mind that this code is based solely on my own perspective
and probably has lots of inaccurate or questionable parts.

This repository is provided as-is without warranty; use the sample code at your own risk.

## Technology Stack

- Language: C#
- Frameworks: .NET (net8.0, net10.0)
- APIs and protocols: gRPC, OpenAPI/Swagger

## Setup

### Prerequisites

- .NET SDK installed
- Optional: Visual Studio Code with Remote - Containers extension
- Optional: Docker for devcontainer usage

### Build

From the repository root:

```bash
dotnet build
```

### Dev Container setup

Before starting the devcontainer, prepare the local SSL certificate files used by the container environment.

1. Run the certificate generation script from the repository root.

   ```bash
   ./.devcontainer/ssl/ssl-cert-generate.sh
   ```

2. After the certificate files are generated in `.devcontainer/ssl`, start the devcontainer.
