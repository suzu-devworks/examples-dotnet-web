# examples-dotnet-web

![Dynamic XML Badge](https://img.shields.io/badge/dynamic/xml?url=https%3A%2F%2Fraw.githubusercontent.com%2Fsuzu-devworks%2Fexamples-dotnet-web%2Frefs%2Fheads%2Fmain%2Fsrc%2FDirectory.Build.props&query=%2F%2FLatestFramework&logo=dotnet&label=Framework&color=%23512bd4)
[![build](https://github.com/suzu-devworks/examples-dotnet-web/actions/workflows/dotnet-build.yml/badge.svg)](https://github.com/suzu-devworks/examples-dotnet-web/actions/workflows/dotnet-build.yml)
[![CodeQL](https://github.com/suzu-devworks/examples-dotnet-web/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/suzu-devworks/examples-dotnet-web/actions/workflows/github-code-scanning/codeql)

## What is this repository?

This repository is a personal knowledge base for ASP.NET and .NET web development.

It collects practical examples, experiments, and implementation notes that are useful when exploring real-world web scenarios.

The code reflects current understanding and may evolve over time.
Use samples at your own risk.

## What topics are covered?

- Authentication and authorization patterns
- ASP.NET Core hosting and request pipeline behavior
- Filters, logging, and infrastructure integrations
- Web API, gRPC, OpenAPI, and Swagger usage
- Web UI examples including Razor Pages, MVC, and Blazor

## Why use Dev Containers?

Using Dev Containers is recommended when working with this repository.
They provide the tools and dependencies required to build and run these examples without changing your local environment.

Configuration details are in [`.devcontainer/devcontainer.json`](.devcontainer/devcontainer.json).

Before starting the container, generate local SSL certificate files:

```bash
./.devcontainer/ssl/ssl-cert-generate.sh
```

After certificates are generated in `.devcontainer/ssl`, start the container.

Once the container is up, run [`.devcontainer/postCreateCommand.sh`](.devcontainer/postCreateCommand.sh)
and follow the terminal instructions.
