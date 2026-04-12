using Examples.Web.WebApi.Grpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Web.Infrastructure.GrpcClient;

/// <summary>
/// Extension methods for registering gRPC clients and related services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds gRPC clients and related services to the dependency injection container based on the provided configuration.
    /// </summary>
    /// <param name="services">The service collection to add the gRPC clients to.</param>
    /// <param name="configuration">The configuration to bind the gRPC client options from.</param>
    /// <returns>The updated service collection.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the gRPC client options cannot be bound from the configuration.</exception>
    public static IServiceCollection AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddTransient<UnhandledExceptionInterceptor>();
        services.AddTransient<LoggingInterceptor>();

        var section = configuration.GetSection(GrpcClientOptions.ConfigurationSectionName);

        services.AddOptions<GrpcClientOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var clientOptions = section.Get<GrpcClientOptions>()
            ?? throw new InvalidOperationException("Failed to bind GrpcClientOptions from configuration.");

        var builder = services.AddGrpcClient<Greeter.GreeterClient>(options =>
        {
            options.Address = new Uri(clientOptions.Greeter.BaseAddress);
        })
        .AddInterceptor<UnhandledExceptionInterceptor>()
        .AddInterceptor<LoggingInterceptor>();

        if (clientOptions.Greeter.AllowUntrustedCertificate)
        {
            builder.ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback
                        = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                return handler;
            });
        }

        services.AddScoped<IGreeterService, Internals.GreeterService>();

        return services;
    }

}
