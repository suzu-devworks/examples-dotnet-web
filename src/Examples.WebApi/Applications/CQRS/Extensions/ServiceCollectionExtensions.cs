using Microsoft.Extensions.DependencyInjection;
using Examples.WebApi.Applications.CQRS.Behaviors;
using MediatR;

namespace Examples.WebApi.Applications.CQRS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCQRS(this IServiceCollection services)
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
}