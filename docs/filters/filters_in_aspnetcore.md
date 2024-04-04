# Filters in ASP.NET Core

## Table of Contents <!-- omit in toc -->

- [Filters in ASP.NET Core](#filters-in-aspnet-core)
  - [References](#references)
  - [Filter types](#filter-types)
  - [Filter attributes](#filter-attributes)


## References

- [Filters in ASP.NET Core](https://learn.microsoft.com/ja-jp/aspnet/core/mvc/controllers/filters?view=aspnetcore-8.0)

## Filter types

Each filter type is executed at a different stage in the filter pipeline:

- Authorization filters
  - [`IAuthorizationFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iauthorizationfilter?view=aspnetcore-8.0)
  - [`IAsyncAuthorizationFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iasyncauthorizationfilter?view=aspnetcore-8.0)

- Resource filters
  - [`IResourceFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iresourcefilter?view=aspnetcore-8.0)
  - [`IAsyncResourceFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iasyncresourcefilter?view=aspnetcore-8.0)
  
- Action filters
  - Are not supported in Razor Pages.
  - [`IActionFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iactionfilter?view=aspnetcore-8.0)
  - [`IAsyncActionFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iasyncactionfilter?view=aspnetcore-8.0)

- Endpoint filters
  - Are not supported in Razor Pages.
  - [`EndpointFilterExtensions.AddEndpointFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.http.endpointfilterextensions.addendpointfilter?view=aspnetcore-8.0)

- Page filters (Razor only)
  - [`IPageFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.ipagefilter?view=aspnetcore-8.0)
  - [`IAsyncPageFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iasyncpagefilter?view=aspnetcore-8.0)

- Exception filters
  - [`IExceptionFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iexceptionfilter?view=aspnetcore-8.0)
  - [`IAsyncExceptionFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iasyncexceptionfilter?view=aspnetcore-8.0)

- Result filters
  - [`IResultFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iresultfilter?view=aspnetcore-8.0)
  - [`IAsyncResultFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iasyncresultfilter?view=aspnetcore-8.0)
  - [`IAlwaysRunResultFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.ialwaysrunresultfilter?view=aspnetcore-8.0)
  - [`IAsyncAlwaysRunResultFilter`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.iasyncalwaysrunresultfilter?view=aspnetcore-8.0)


## Filter attributes

ASP.NET Core includes built-in attribute-based filters that can be subclassed and customized

- [`ActionFilterAttribute`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.actionfilterattribute?view=aspnetcore-8.0)
- [`ExceptionFilterAttribute`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.exceptionfilterattribute?view=aspnetcore-8.0)
- [`ResultFilterAttribute`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.filters.resultfilterattribute?view=aspnetcore-8.0)
- [`FormatFilterAttribute`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.formatfilterattribute?view=aspnetcore-8.0)
- [`ServiceFilterAttribute`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.servicefilterattribute?view=aspnetcore-8.0)
- [`TypeFilterAttribute`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.typefilterattribute?view=aspnetcore-8.0)
