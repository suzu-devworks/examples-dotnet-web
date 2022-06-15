using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Examples.WebApi.Infrastructure.Locatization;

namespace Examples.WebApi.Applications.Localization
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomeLocalization(this IServiceCollection services)
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
}
