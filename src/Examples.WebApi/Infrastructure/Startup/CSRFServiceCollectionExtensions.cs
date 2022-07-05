using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.WebApi.Infrastructure.Startup;

/// <summary>
/// /// Extension methods for adding CSRF/XSRF to an <see cref="IServiceCollection" />.
/// </summary>
public static class CSRFServiceCollectionExtensions
{
    public static IServiceCollection AddCustokmAntiforgery(this IServiceCollection services, string headerName)
    {
        // services.AddControllersWithViews(options =>
        //     options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

        services.AddAntiforgery(options =>
            options.HeaderName = headerName);

        services.AddSwaggerGen(options =>
            options.OperationFilter<CSRFTokenParameterAppenderFilter>(headerName));

        return services;
    }

}
