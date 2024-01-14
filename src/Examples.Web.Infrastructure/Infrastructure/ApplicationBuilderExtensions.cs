using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Examples.Web.Infrastructure;

/// <summary>
/// Extension methods for infrastructures to <see cref="IApplicationBuilder" />.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Sets the security values to HTTP header.
    /// </summary>
    /// <param name="app"></param>
    /// /// <returns></returns>
    public static IApplicationBuilder UseSecurityHttpResponseHeader(this IApplicationBuilder app)
    {
        app.UseMiddleware<Middleware>();

        return app;
    }


    private class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.TryAdd("Content-Security-Policy", "default-src 'self';frame-ancestors 'none'");
            context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
            context.Response.Headers.TryAdd("X-Xss-Protection", "1; mode=block");
            context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");

            // Call the next delegate/middleware in the pipeline.
            await _next.Invoke(context);

            return;
        }

    }

}
