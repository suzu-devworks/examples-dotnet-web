# Examples.Web.Blazor.WebApp

## Table of Contents <!-- omit in toc -->

- [Overview](#overview)
- [Examples](#examples)
  - [JavaScript Interop](#javascript-interop)
  - [Components](#components)
  - [Tutorials](#tutorials)
- [Development](#development)
  - [Build](#build)
  - [Run](#run)
  - [How the project was initialized](#how-the-project-was-initialized)

## Overview

This project is intended for testing and learning the functionality of the ASP.NET Blazor Web App.

## Examples

### JavaScript Interop

- [ASP.NET Core Blazor JavaScript interoperability (JS interop)](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/javascript-interoperability/)

I'm experimenting with calling JavaScript from .NET using [`Interop.razor`](./Components/Pages/Interop.razor). Currently, I'm only using `console.log`, but I plan to use it for other purposes in the future.

Also, I'm trying to use the Chart.js library from .NET, following the instructions on the [`Weather.razor`](./Components/Pages/Weather.razor) page. I'm proceeding by referring to various websites.

### Components

- [Modify a component](https://dotnet.microsoft.com/ja-jp/learn/aspnet/blazor-tutorial/modify)

I plan to add arguments to the [`Counter.razor`](./Components/Pages/Counter.razor) following the tutorial, but I might as well create it as a component. However, be careful, as using a component with the same name as the page will cause an error.

### Tutorials

- [Build a Blazor todo list app](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/tutorials/build-a-blazor-app)

## Development

### Build

Build this project from the repository root:

```shell
dotnet build src/Examples.Web.Blazor.WebApp/Examples.Web.Blazor.WebApp.csproj
```

### Run

Run this project from the repository root:

```shell
dotnet run --project src/Examples.Web.Blazor.WebApp/Examples.Web.Blazor.WebApp.csproj -lp https
```

### How the project was initialized

This project was initialized with the following command:

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Blazor.WebApp
dotnet new blazor -o src/Examples.Web.Blazor.WebApp
dotnet sln add src/Examples.Web.Blazor.WebApp/
cd src/Examples.Web.Blazor.WebApp

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```
