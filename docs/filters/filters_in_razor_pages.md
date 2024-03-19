# Filter methods for Razor Pages in ASP.NET Core

## Table of Contents <!-- omit in toc -->

- [Filter methods for Razor Pages in ASP.NET Core](#filter-methods-for-razor-pages-in-aspnet-core)
  - [References](#references)
  - [Filter types](#filter-types)
  - [Filter order](#filter-order)


## References

- [Filter methods for Razor Pages in ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/razor-pages/filter?view=aspnetcore-8.0)

## Filter types

Razor Page filters:

- Run code after a handler method has been selected, but before model binding occurs.
- Run code before the handler method executes, after model binding is complete.
- Run code after the handler method executes.
- Can be implemented on a page or globally.
- **Cannot be applied to specific page handler methods.**
- Can have constructor dependencies populated by Dependency Injection (DI). For more information, see ServiceFilterAttribute and TypeFilterAttribute.


- Page filters
  - [`IPageFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.ipagefilter?view=aspnetcore-8.0)
  - [`IAsyncPageFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iasyncpagefilter?view=aspnetcore-8.0)

## Filter order

sync filters:

- IResourceFilter.OnResourceExecuting
  - IPageFilter.OnPageHandlerSelected
  - IPageFilter.OnPageHandlerExecuting
    - *RazorPage.Handled*
  - IPageFilter.OnPageHandlerExecuted
  - IResultFilter.OnResultExecuting
    - *Result execution*
  - IResultFilter.OnResultExecuted
- IResourceFilter.OnResourceExecuted

async filters:

- IAsyncResourceFilter.OnResourceExecutionAsync (before next)
  - IAsyncPageFilter.OnPageHandlerSelectionAsync
  - IAsyncPageFilter.OnPageHandlerExecutionAsync (before next)
    - *RazorPage.Handled*
  - IAsyncPageFilter.OnPageHandlerExecutionAsync (after next)
  - IAsyncResultFilter.OnResultExecutionAsync (before next)
    - *Result execution*
  - IAsyncResultFilter.OnResultExecutionAsync (after next)
- IAsyncResourceFilter.OnResourceExecutionAsync (after next)

