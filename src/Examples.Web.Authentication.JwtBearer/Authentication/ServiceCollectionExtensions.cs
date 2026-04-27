using Examples.Web.Authentication.Services;

namespace Examples.Web.Authentication;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRevocableJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtBlacklistOptions>(configuration);

        services.AddSingleton<ITokenBlacklistService, ConfigurationTokenBlacklistService>();

        return services;
    }

}
