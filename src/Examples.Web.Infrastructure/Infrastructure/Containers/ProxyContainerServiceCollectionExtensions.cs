#if NET10_0_OR_GREATER

using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Web.Infrastructure.Containers;

/// <summary>
/// Extension methods for container services to <see cref="IServiceCollection"/>.
/// </summary>
public static class ProxyContainerServiceCollectionExtensions
{
    /// <summary>
    /// Adds forwarded headers configuration to the service collection,
    /// allowing the application to process X-Forwarded-For, X-Forwarded-Proto, and X-Forwarded-Prefix headers
    /// from trusted proxies.
    /// </summary>
    /// <param name="services">The service collection to add the configuration to.</param>
    /// <param name="trustedProxyNetwork">The trusted proxy network. If null, defaults to the Docker bridge network (172.16.0.0/12).</param>
    public static void AddContainerForwardedHeaders(this IServiceCollection services,
        System.Net.IPNetwork? trustedProxyNetwork = null)
    {
        // Allows the all range of docker bridge network (172.16.0.0/12) to be trusted for forwarded headers.
        trustedProxyNetwork ??= new(IPAddress.Parse("172.16.0.0"), 12);

        // Add Forwarded Headers Middleware to process the X-Forwarded-For, X-Forwarded-Proto, and X-Forwarded-Prefix headers.
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.KnownProxies.Clear();
            options.KnownIPNetworks.Clear();
            options.KnownIPNetworks.Add(trustedProxyNetwork.Value);

            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedPrefix;
        });
    }
}

#endif
