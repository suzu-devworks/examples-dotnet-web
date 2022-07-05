using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace Examples.WebApi.Infrastructure.Startup;

/// <summary>
/// Extension methods for HTTP Header security values.
/// </summary>
public static class HttpHeaderSecurityApplicationBuilderExtensions
{
    /// <summary>
    /// Sets the security values to HTTP header.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCustomHttpHeaderSecurity(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            context.Response.Headers.TryAdd("Content-Security-Policy", "default-src 'self';frame-ancestors 'none'");
            context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
            context.Response.Headers.TryAdd("X-Xss-Protection", "1; mode=block");
            context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");

            await next();
        });

        return app;
    }

}
