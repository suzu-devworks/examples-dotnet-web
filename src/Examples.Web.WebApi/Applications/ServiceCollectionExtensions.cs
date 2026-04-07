using Examples.WebApi.Applications.Localization;

namespace Examples.Web.WebApi.Applications;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationsServices(this IServiceCollection services)
    {
        services.AddLocalizationExample();

        return services;
    }

}
