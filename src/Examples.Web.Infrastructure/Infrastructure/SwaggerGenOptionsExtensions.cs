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
    /// <param name="name"></param>
    /// <returns></returns>
    /// <see href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file#add-security-definitions-and-requirements-for-bearer-auth" />
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

}
