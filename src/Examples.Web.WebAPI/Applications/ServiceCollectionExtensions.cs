using Examples.WebAPI.Applications.Localization;

namespace Examples.Web.WebAPI.Applications;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationsServices(this IServiceCollection services)
    {
        services.AddLocalizationExample();

        return services;
    }

}