using System.Reflection;

namespace Examples.Web.Infrastructure.Grpc;

/// <summary>
/// Provides extension methods for configuring gRPC services on IApplicationBuilder.
/// </summary>
public static class ApplicationBuilderExtensions
{
    private static readonly MethodInfo MapGrpcServiceMethod = ResolveMapGrpcServiceMethod();

    /// <summary>
    /// Maps all gRPC services from the assembly containing TAssemblyMarker.
    /// </summary>
    /// <typeparam name="TAssemblyMarker">A type from the assembly to scan for gRPC services.</typeparam>
    /// <param name="app">The application builder.</param>
    /// <returns>The application builder for chaining.</returns>
    public static IApplicationBuilder MapGrpcServices<TAssemblyMarker>(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var assembly = typeof(TAssemblyMarker).Assembly;
        return app.MapGrpcServices<TAssemblyMarker>(assembly);
    }

    /// <summary>
    /// Maps all gRPC services from the specified assembly.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="assembly">The assembly to scan for gRPC services.</param>
    /// <returns>The application builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if app or assembly is null.</exception>
    public static IApplicationBuilder MapGrpcServices<TAssemblyMarker>(this IApplicationBuilder app, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(assembly);

        foreach (var mappedGrpcServiceMethod in ResolveMapGrpcServiceMethods(assembly))
        {
            mappedGrpcServiceMethod.Invoke(null, [app]);
        }

        return app;
    }

    private static MethodInfo ResolveMapGrpcServiceMethod()
    {
        return typeof(GrpcEndpointRouteBuilderExtensions).GetMethods()
            .FirstOrDefault(m => m.Name == nameof(GrpcEndpointRouteBuilderExtensions.MapGrpcService)
                && m.IsGenericMethod
                && m.GetParameters().Length == 1)
            ?? throw new InvalidOperationException($"Unable to find method {nameof(GrpcEndpointRouteBuilderExtensions.MapGrpcService)} on type {typeof(GrpcEndpointRouteBuilderExtensions).FullName}.");
    }

    private static IEnumerable<MethodInfo> ResolveMapGrpcServiceMethods(Assembly assembly)
    {
        return ResolveGrpcServiceTypes(assembly)
            .Select(serviceType => MapGrpcServiceMethod.MakeGenericMethod(serviceType));
    }

    private static IEnumerable<Type> ResolveGrpcServiceTypes(Assembly assembly)
    {
        return assembly.GetExportedTypes()
            .Where(t => t.IsClass && !t.IsAbstract && IsGrpcServiceImplementation(t));
    }

    private static bool IsGrpcServiceImplementation(Type type)
    {
        return type.BaseType is { IsAbstract: true } baseType &&
               baseType.IsDefined(typeof(global::Grpc.Core.BindServiceMethodAttribute), false);
    }
}
