using Examples.Web.Infrastructure.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.WebAPI.Applications.Localization;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalizationExample(this IServiceCollection services)
    {
        // Add Resource file search path.
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.AddMvc()
            // use Razor localization.
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            // use Annotation localization.
            .AddDataAnnotationsLocalization(options =>
                // use custom IStringLocalizer (aggregation).
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    StringLocalizerAggregator.Create(localizer =>
                    {
                        localizer.Add(factory.Create(typeof(SharedResource)));
                        localizer.Add(factory.Create(type));
                    }));

        return services;
    }

}