using Examples.Web.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Examples.Web.Infrastructure;

public static class SwaggerGenOptionsExtensions
{
    public static SwaggerGenOptions UseCustomSwagger(this SwaggerGenOptions options)
    {
        options.EnableAnnotations();
        options.OperationFilter<RequestHeaderParameterOperationFilter>();

        return options;
    }

}
