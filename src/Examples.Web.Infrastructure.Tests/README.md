# Examples.Web.Infrastructure.Tests

## Table of Contents <!-- omit in toc -->

- [Development](#development)
  - [How the project was initialized](#how-the-project-was-initialized)

## Development

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Infrastructure
dotnet new classlib -o src/Examples.Web.Infrastructure
dotnet sln add src/Examples.Web.Infrastructure/
cd src/Examples.Web.Infrastructure
dotnet add package Swashbuckle.AspNetCore
dotnet add package Swashbuckle.AspNetCore.Annotations
cd ../../

## Examples.Web.Infrastructure.Tests
dotnet new xunit -o src/Examples.Web.Infrastructure.Tests
dotnet sln add src/Examples.Web.Infrastructure.Tests/
cd src/Examples.Web.Infrastructure.Tests
dotnet add reference ../Examples.Web.Infrastructure
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package coverlet.collector
dotnet add package Moq
dotnet add package ChainingAssertion.Core.Xunit
cd ../../

# Update outdated package
dotnet list package --outdated
```
