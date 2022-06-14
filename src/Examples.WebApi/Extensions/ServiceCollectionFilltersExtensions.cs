using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Examples.WebApi.Infrastructure.Filters;

namespace Examples.WebApi.Infrastructure.Extensions
{
    public static class ServiceCollectionFilltersExtensions
    {
        public static IServiceCollection AddCustomFilters(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PositionOptions>(configuration.GetSection("Position"));
            services.AddScoped<MyActionFilterAttribute>();

            services.AddScoped<MyActionTypeFilterAttribute>();
            services.AddScoped<AddHeaderResultServiceFilter>();

            services.AddControllersWithViews(options =>
            {
                //Add with DI.
                options.Filters.Add<MyActionFilter>();
                options.Filters.Add<MyAsyncActionFilter>();
                options.Filters.Add<MyAsyncResultFilter>();
                options.Filters.Add<MyAsyncResourceFilter>();

                options.Filters.Add<CurlLoggingActionFilter>();
            });

            return services;
        }

    }
}