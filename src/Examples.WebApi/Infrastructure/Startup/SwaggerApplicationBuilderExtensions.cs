using System;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Examples.WebApi.Infrastructure.Startup;

/// <summary>
/// Extension methods for custom Swagger services.
/// </summary>
public static class SwaggerApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomSwaggerUI(this IApplicationBuilder app, Action<SwaggerUIOptions>? setupAction = null)
    {
        app.UseSwaggerUI(c =>
        {
            setupAction?.Invoke(c);

            // schema models is not expand.
            c.DefaultModelsExpandDepth(0);

            // c.UseRequestInterceptor(@"(req) => { req.headers['X-XSRF-TOKEN'] = localStorage.getItem('xsrf-token'); return req; }");
            // c.UseResponseInterceptor("(res) => { console.log('Custom interceptor intercepted response from:', res.url); return res; }");

        });

        return app;
    }
}
