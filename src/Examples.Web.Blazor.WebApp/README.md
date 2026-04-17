# Examples.Web.Blazor.WebApp

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [Examples](#examples)
  - [JavaScript Interop](#javascript-interop)
  - [Components](#components)
  - [State](#state)
  - [Tutorials](#tutorials)
- [Development](#development)
  - [Build](#build)
  - [Run](#run)
  - [How the project was initialized](#how-the-project-was-initialized)

## Overview

This project is intended for experimenting with and learning the features of ASP.NET Blazor Web Apps.

## Examples

### JavaScript Interop

- [ASP.NET Core Blazor JavaScript interoperability (JS interop)](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/javascript-interoperability/)

I'm experimenting with calling JavaScript from .NET using [`Interop.razor`](./Components/Pages/Interop.razor). Right now, I'm only using `console.log`, but I plan to use it for other purposes in the future.

Also, I'm trying to use the [`Chart.js`](https://www.chartjs.org/) library from .NET by following the guidance on [`Weather.razor`](./Components/Pages/Weather.razor).

### Components

- [Modify a component](https://dotnet.microsoft.com/ja-jp/learn/aspnet/blazor-tutorial/modify)

Instead of adding parameters to [`Counter.razor`](./Components/Pages/Counter.razor), I tried extracting part of it into a separate component and testing that approach. However, be careful, because using a component with the same name as the page will cause an error.

### State

- [ASP.NET Core Blazor protected browser storage](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/state-management/protected-browser-storage)

I'm currently experimenting with both session storage and local storage to preserve the counter state in [`Counter.razor`](./Components/Pages/Counter.razor).

### Tutorials

- [Build a Blazor todo list app](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/tutorials/build-a-blazor-app)

## Development

### Build

Build this project from the repository root:

```shell
dotnet build src/Examples.Web.Blazor.WebApp/
```

### Run

Run this project from the repository root:

```shell
dotnet run --project src/Examples.Web.Blazor.WebApp/ -lp https
```

### How the project was initialized

This project was initialized with the following commands:

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Blazor.WebApp
dotnet new blazor -o src/Examples.Web.Blazor.WebApp
dotnet sln add src/Examples.Web.Blazor.WebApp/
cd src/Examples.Web.Blazor.WebApp

dotnet user-secrets init
cd ../../

# Check outdated packages
dotnet list package --outdated
```
