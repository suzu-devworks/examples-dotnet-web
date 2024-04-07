# Customizing Swagger

## Table of contents <!-- omit in toc -->

- [Customizing Swagger](#customizing-swagger)
  - [References](#references)
  - [Customizations](#customizations)
    - [Change the Swagger JSON file format to yaml](#change-the-swagger-json-file-format-to-yaml)
    - [Collapse document](#collapse-document)
    - [Include Descriptions from XML Comments](#include-descriptions-from-xml-comments)
    - [Provide Global API Metadata](#provide-global-api-metadata)
    - [EnableAnnotations](#enableannotations)
    - [Add authentication button](#add-authentication-button)
    - [Hide parameters](#hide-parameters)
    - [Hide operation](#hide-operation)
    - [Grouping Operations With Tags](#grouping-operations-with-tags)
  - [Troubleshooting](#troubleshooting)
    - [does not contain an entry point.](#does-not-contain-an-entry-point)


## References

- [Swashbuckle.AspNetCore - github](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

## Customizations

### Change the Swagger JSON file format to yaml

The file format of Swagger doc can be specified with SwaggerEndpoint.

```cs
  app.UseSwaggerUI(options =>
  {
      //# swagger document generated to json and yaml.
      options.SwaggerEndpoint("v1/swagger.json", "Examples.Web.WebAPI v1");
      options.SwaggerEndpoint("v1/swagger.yaml", "Examples.Web.WebAPI v1(yaml)");
  });
```

### Collapse document

Use `DefaultModelsExpandDepth` when collapsing the schema.<br>
Use `DocExpansion` when collapsing an API.

```cs
  app.UseSwaggerUI(options =>
  {
      //# Schemas shrink all.
      options.DefaultModelsExpandDepth(0);
      //options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
  });
```

### Include Descriptions from XML Comments

As separated into extension methods:
```cs
  builder.Services.AddSwaggerGen(options =>
  {
      options.UseXmlComments();
  });
```

```cs
public static SwaggerGenOptions UseXmlComments(this SwaggerGenOptions options, string? xmlFilePath = default)
{
    xmlFilePath ??= $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml";
    var path = Path.Combine(AppContext.BaseDirectory, xmlFilePath);
    options.IncludeXmlComments(path);

    return options;
}
```

### Provide Global API Metadata

Display various things in the header part of swagger ui.

```cs
  builder.Services.AddSwaggerGen(options =>
  {
      options.SwaggerDoc("v1", new()
      {
          Version = "v1",
          Title = "Examples.Web.WebAPI",
          Description = "&#127861; ASP.NET Core Web API examples.",
          License = new OpenApiLicense
          {
              Name = "MIT License",
              Url = new Uri("https://github.com/suzu-devworks/examples-dotnet-web/blob/main/LICENSE")
          }
      });
  });
```

### EnableAnnotations

Enable `Swashbuckle.AspNetCore.Annotations`.

```cs
builder.Services.AddSwaggerGen(options => 
        options.EnableAnnotations()
        );
```

These attributes will be available:

- `SwaggerOperationAttribute`
  - use `AnnotationsOperationFilter`

- `SwaggerResponseAttribute`
  - use `AnnotationsOperationFilter`

- `SwaggerParameterAttribute`
  - decorated with [FromRoute], [FromQuery] or [FromHeader])
  - use `AnnotationsParameterFilter`

- `SwaggerRequestBodyAttribute`
  - decorated with [FromBody])
  - use `AnnotationsRequestBodyFilter`

- `SwaggerSchemaFilterAttribute`
  - use `AnnotationsSchemaFilter`

- `SwaggerOperationFilterAttribute`
  - use `AnnotationsOperationFilter`

- `SwaggerTagAttribute`
  - use `AnnotationsDocumentFilter`


### Add authentication button

As separated into extension methods:

```cs
builder.Services.AddSwaggerGen(options => 
        options.UseJWTBearerAuthorization()
        );
```

An example of JWT bearer authentication is:

```cs
public static SwaggerGenOptions UseJWTBearerAuthorization(this SwaggerGenOptions options, string name = "BearerAuth")
{
    options.AddSecurityDefinition(name, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    // options.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {
    //     {
    //         new OpenApiSecurityScheme
    //         {
    //             Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = name }
    //         },
    //         new string[] {}
    //     }
    // });
    
    options.OperationFilter<AuthenticationRequestOperationFilter>(name);

    return options;
}
```

You can enable the commented out part, but I think it's hard to understand because even APIs that don't require authentication are marked with a lock mark.

So I will create a filter that will mark only the parts that require authentication.

- [AuthenticationRequestOperationFilter](/src/Examples.Web.Infrastructure/Infrastructure/Swagger/AuthenticationRequestOperationFilter.cs)

The samples we often see use `AuthorizeAttribute`

```cs
    var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
        .Union(context.MethodInfo.GetCustomAttributes(true))
        .OfType<AuthorizeAttribute>();
```

However, in this case, I couldn't support the pattern of setting the default to Authorize and adding AllowAnonymousAttribute, 
so I looked at the metadata.


### Hide parameters

Specify parameters that you do not want to appear in the Swagger UI.

- [HideParameterOperationFilter](/src/Examples.Web.Infrastructure/Infrastructure/Swagger/HideParameterOperationFilter.cs)

In use:

```cs
  public record Parameter
  {
      public int Start { get; set; }

      public int End { get; set; }

      [HideParameter]
      public bool HasRange => (Start > 0) && (End > 0);
  }
```

### Hide operation

Use `ApiExplorerSettingsAttribute` to hide the operation.

```cs
  [ApiExplorerSettings(IgnoreApi = true)]
  [AllowAnonymous]
  [HttpPost("echo")]
  public async Task<IActionResult> EchoAsync([FromBody] string message, CancellationToken cancellationToken)
  {
      await Task.Delay(0, cancellationToken);
      return Ok(new { message });
  }
```

### Grouping Operations With Tags

By default, operations are grouped by controller and displayed in order by controller name.

If you want to split this into other groups, there are tags in the OpenAPI specification.

- [Grouping Operations With Tags](https://swagger.io/docs/specification/2-0/grouping-operations-with-tags/?sbsearch=grouping)

There are two attributes that OpenApiTag can specify in Swagger:

- `SwaggerOperationAttribute`
- `SwaggerTagAttribute`

However, the places that can be specified for these two are different; `SwaggerOperationAttribute` can only be specified for methods, and `SwaggerTagAttribute` can only be specified for classes.

As expected, the locations defined in each SwaggerDoc are different.

SwaggerOperationAttribute is an optional tag for each operation

```cs:AnnotationsOperationFilter
  operation.Tags = swaggerOperationAttribute.Tags
                .Select(tagName => new OpenApiTag { Name = tagName })
                .ToList();
```

SwaggerTagAttribute is the document root tag

```cs:AnnotationsOperationFilter
  swaggerDoc.Tags.Add(new OpenApiTag
  {
      Name = controllerName,
      Description = swaggerTagAttribute.Description,
      ExternalDocs = (swaggerTagAttribute.ExternalDocsUrl != null)
          ? new OpenApiExternalDocs { Url = new Uri(swaggerTagAttribute.ExternalDocsUrl) }
          : null
  });
```

When using SwaggerTagAttribute, OpenApiTag.Name remains the controller name, so if you want to change it, you will need to create a separate Filter.

However, grouping using Description is easier than this.

- [OpenApiTagDescriptionSortDocumentFilter](/src/Examples.Web.Infrastructure/Infrastructure/Swagger/OpenApiTagDescriptionSortDocumentFilter.cs)


## Troubleshooting

### does not contain an entry point.

This error has been observed to occur whenever Swashbuckle.AspnetCore is added to a clslib project.

- https://github.com/dotnet/aspnetcore/issues/14370

```console
error : Assembly '/workspaces/examples-dotnet-web/src/Examples.Web.Infrastructure/bin/Debug/net7.0/Examples.Web.Infrastructure.dll' does not contain an entry point. 

exited with code 3. [/workspaces/examples-dotnet-web/src/Examples.Web.Infrastructure/Examples.Web.Infrastructure.csproj::TargetFramework=net7.0]

```

Configure the project to not generate Swagger documents.

```xml
  <PropertyGroup>
    <OpenApiGenerateDocuments>false</OpenApiGenerateDocuments>
  </PropertyGroup>
```
