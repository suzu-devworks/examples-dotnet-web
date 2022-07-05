using Microsoft.Extensions.DependencyInjection;

namespace Examples.WebApi.Infrastructure.Startup;

/// <summary>
/// /// Extension methods for adding CORF to an <see cref="IServiceCollection" />.
/// </summary>
public static class CORSServiceCollectionExtensions
{
    public static IServiceCollection AddCustomCorsPolicy(this IServiceCollection services, string origin)
    {
        services.AddCors(options =>
            options.AddPolicy(name: CorsPolicyDefines.SPA_POLICY_NAME, policy =>
                policy.WithOrigins(origin)
                    .AllowAnyMethod()
                    //.WithMethods("GET", "POST")
                    //.AllowAnyHeader()
                    .WithHeaders("X-REQUESTED-WITH")
                    .AllowCredentials()
        ));

        return services;
    }

}
