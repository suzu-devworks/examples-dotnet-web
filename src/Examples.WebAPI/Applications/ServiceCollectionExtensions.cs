using Examples.WebAPI.Applications.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.WebAPI.Applications;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLocalizationExample();

        return services;
    }

}
