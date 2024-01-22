using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Examples.Web.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.Web.Infrastructure;

public static class SwaggerGenOptionsExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static SwaggerGenOptions UseAnnotationFilters(this SwaggerGenOptions options)
    {
        options.EnableAnnotations();
        options.DocumentFilter<OrderTagsDocumentFilter>();

        options.OperationFilter<HiddenParameterOperationFilter>();
        options.OperationFilter<RequestHeaderParameterAppendingOperationFilter>();
        options.OperationFilter<ResponseHeaderAppendingOperationFilter>();

        return options;
    }

    /// <summary>
    /// Adding triple-slash comments to an action enhances the Swagger UI
    /// by adding the description to the section header.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="xmlFilePath"></param>
    /// <returns></returns>
    public static SwaggerGenOptions UseXmlComments(this SwaggerGenOptions options, string? xmlFilePath = default)
    {
        xmlFilePath ??= $"{Assembly.GetCallingAssembly().GetName().Name}.xml";
        var path = Path.Combine(AppContext.BaseDirectory, xmlFilePath);
        if (File.Exists(path))
        {
            options.IncludeXmlComments(path);
        }

        return options;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <see href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore#add-security-definitions-and-requirements-bearer" />
    public static SwaggerGenOptions AddJWTBearerAuthorization(this SwaggerGenOptions options, string name = "BearerAuth")
    {
        options.AddSecurityDefinition(name, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        });

        options.OperationFilter<AuthenticationRequestOperationFilter>(name);

        return options;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static SwaggerGenOptions AddJWTBearerAuthorizationWithApiKey(this SwaggerGenOptions options, string name = "BearerAPI")
    {
        options.AddSecurityDefinition(name, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
        });

        options.OperationFilter<AuthenticationRequestOperationFilter>(name);

        return options;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    /// <param name="headerName"></param>
    /// <returns></returns>
    public static SwaggerGenOptions AddAntiforgery(this SwaggerGenOptions options, string? headerName)
    {
        options.OperationFilter<AntiforgeryTokenParameterAppendingFilter>(headerName);

        return options;
    }

}
