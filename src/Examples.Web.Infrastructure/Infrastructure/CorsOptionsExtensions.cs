using System;
using System.Linq;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Examples.Web.Infrastructure.Security;

/// <summary>
/// Extension methods for adding CORF to an <see cref="CorsOptions" />.
/// </summary>
public static class CorsOptionsExtensions
{
    public static CorsOptions AddDefaultSpaPolicyFrom(this CorsOptions options, IConfiguration configuration)
        => options.AddSpaPolicyFrom(options.DefaultPolicyName, configuration);

    public static CorsOptions AddSpaPolicyFrom(this CorsOptions options, string name, IConfiguration configuration)
    {
        var origins = configuration.GetSection("Origins")
            .GetChildren().Select(x => x.Value).ToArray();

        if (!origins.Any())
        {
            throw new InvalidOperationException("Cors Origins is not defined.");
        }

        var exposedHeaders = configuration.GetSection("ExposedHeaders")
            .GetChildren().Select(x => x.Value).ToArray();

        options.AddPolicy(name, policy =>
            policy.WithOrigins(origins!)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders(exposedHeaders!)
                .AllowCredentials()
            );

        return options;
    }
}
