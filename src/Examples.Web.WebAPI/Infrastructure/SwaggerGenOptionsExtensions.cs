using Examples.Web.Infrastructure.Swagger;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.Web.Infrastructure;

public static class SwaggerGenOptionsExtensions
{
    public static SwaggerGenOptions UseCustomSwagger(this SwaggerGenOptions options)
    {
        options.EnableAnnotations();
        options.DocumentFilter<OpenApiTagDescriptionSortDocumentFilter>();

        options.OperationFilter<HideParameterOperationFilter>();
        options.OperationFilter<RequestHeaderParameterOperationFilter>();
        options.OperationFilter<RequestHeaderParameterOperationFilter>();

        options.MapType<TimeSpan>(() => new() { Type = "string" });

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

        return options;
    }

}