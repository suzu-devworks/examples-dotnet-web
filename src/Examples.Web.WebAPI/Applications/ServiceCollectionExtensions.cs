using Examples.WebAPI.Applications.Localization;

namespace Examples.Web.WebAPI.Applications;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLocalizationExample();

        return services;
    }

}
