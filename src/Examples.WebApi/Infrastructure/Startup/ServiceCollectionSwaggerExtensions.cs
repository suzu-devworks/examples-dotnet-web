using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Examples.WebApi.Infrastructure.Startup;

/// <summary>
/// Extension methods for adding custom Swagger services to an <see cref="IServiceCollection" />.
/// </summary>
/// <remarks>
/// <see href="https://docs.microsoft.com/ja-jp/aspnet/core/tutorials/getting-started-with-swashbuckle" >Get started with Swashbuckle and ASP.NET Core - Microsoft Docs</see>
/// </remarks>
public static class ServiceCollectionSwaggerExtensions
{
    public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Examples.WebApi",
                Version = "v1",
                Description = "&#x1F376; ASP.NET CoreWeb API for example.",
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://github.com/suzu-devworks/examples-dotnet-web/blob/main/LICENSE")
                }
            });

            // Adding triple-slash comments to an action enhances the Swagger UI
            // by adding the description to the section header.
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlpath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            options.IncludeXmlComments(xmlpath);

        });

        return services;
    }
}
