using Examples.WebApi.Applications.Localization;
using Examples.WebApi.Infrastructure.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.WebApi.Infrastructure.Startup;

/// <summary>
/// Extension methods for adding custom localization services to an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionLocalizationExtensions
{
    public static IServiceCollection AddCustomLocalization(this IServiceCollection services)
    {
        // Add Resource file search path.
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        _ = services.AddMvc()
            // use Razor localization.
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            // use Annotaion localization.
            .AddDataAnnotationsLocalization(options =>
                // use custom IStringLocalizer (aggregation).
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    StringLocalizerAggregator.Create(localizers =>
                    {
                        localizers.Add(factory.Create(typeof(SharedResource)));
                        localizers.Add(factory.Create(type));
                    }));

        return services;
    }


}

