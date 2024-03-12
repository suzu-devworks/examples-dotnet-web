# URLs case in controller routing

## Table of contents <!-- omit in toc -->

- [URLs case in controller routing](#urls-case-in-controller-routing)
  - [Configurations](#configurations)
    - [To Lower-case URLs.](#to-lower-case-urls)
    - [To Kebab-case URLs.](#to-kebab-case-urls)


## Configurations

### To Lower-case URLs.

If you just want everything to be lowercase, just set LowercaseUrls.

When configuring with AddRouting, there seems to be no need to be particularly aware of the configuration order.

```cs
    //# Set lower-case URLs.
    builder.Services.AddRouting(option => option.LowercaseUrls = true);
```

Can also be set with `Configure<RouteOptions>`.

```cs
    //# Set lower-case URLs.
    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
```

In that case, RouteOptions can be bound from Configuration. But it doesn't seem to make much sense.

```cs
    builder.Services.Configure<RouteOptions>(option =>
        builder.Configuration.GetSection("RouteOptions").Bind(option));
```


### To Kebab-case URLs.

A code is required for the kebab case, but it is described in the official documentation.

- [Use a parameter transformer to customize token replacement](https://learn.microsoft.com/ja-jp/aspnet/core/mvc/controllers/routing?view=aspnetcore-8.0#use-a-parameter-transformer-to-customize-token-replacement)

```cs
    builder.Services.AddControllers(options =>
    {
        //# Set kebab-case URLs.
        options.Conventions.Add(new RouteTokenTransformerConvention(
            new SlugifyParameterTransformer()));
    });
```
