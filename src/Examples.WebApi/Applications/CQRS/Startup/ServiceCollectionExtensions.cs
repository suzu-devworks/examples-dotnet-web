using Microsoft.Extensions.DependencyInjection;
using Examples.WebApi.Applications.CQRS.Behaviors;
using MediatR;

namespace Examples.WebApi.Applications.CQRS.Startup;

/// <summary>
/// Extension methods for add services to <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds CQRS Application services.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCQRSApplication(this IServiceCollection services)
    {
        // Implementation classes for the following interfaces are automatically
        // registered by searching within the assembly.
        // - IRequestHandler<,>
        // - INotificationHandler<>
        // - IRequestPreProcessor<>
        // - IRequestPostProcessor<,>
        // - IRequestExceptionHandler<,,>
        // - IRequestExceptionAction<,>
        services.AddMediatR(typeof(ServiceCollectionExtensions));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }

}
