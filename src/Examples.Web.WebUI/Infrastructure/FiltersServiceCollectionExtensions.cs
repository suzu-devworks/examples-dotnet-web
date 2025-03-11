using Examples.Web.Infrastructure.Filters.ProcessingOrder.Actions;
using Examples.Web.Infrastructure.Filters.ProcessingOrder.Exceptions;
using Examples.Web.Infrastructure.Filters.ProcessingOrder.Pages;
using Examples.Web.Infrastructure.Filters.ProcessingOrder.Resource;
using Examples.Web.Infrastructure.Filters.ProcessingOrder.Results;

namespace Examples.Web.Infrastructure;

/// <summary>
/// /// Extension methods for adding custom HTTP filter to an <see cref="IServiceCollection" />.
/// </summary>
public static class FiltersServiceCollectionExtensions
{
    public static IServiceCollection AddProcessingOrderFilters(this IServiceCollection services)
    {
        //# Global filters.

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add<LoggingResourceFilter>();
            options.Filters.Add<LoggingAsyncResourceFilter>();

            options.Filters.Add<LoggingActionFilter>();
            options.Filters.Add<LoggingAsyncActionFilter>();

            options.Filters.Add<LoggingExceptionFilter>();
            options.Filters.Add<LoggingAsyncExceptionFilter>();

            options.Filters.Add<LoggingResultFilter>();
            options.Filters.Add<LoggingAsyncResultFilter>();
            options.Filters.Add<LoggingAlwaysRunResultFilter>();
            options.Filters.Add<LoggingAsyncAlwaysRunResultFilter>();
        });

        services.AddRazorPages()
            .AddMvcOptions(options =>
            {
                options.Filters.Add<LoggingPageFilter>();
                options.Filters.Add<LoggingAsyncPageFilter>();
            });

        //# for use with ServiceFilterAttributes or TypeFilterAttribute.
        services.AddScoped<LoggingAsyncResultFilter>();
        services.AddScoped<LoggingAsyncPageFilter>();

        return services;
    }

}