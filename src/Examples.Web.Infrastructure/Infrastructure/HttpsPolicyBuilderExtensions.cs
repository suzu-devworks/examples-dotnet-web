using Microsoft.AspNetCore.Builder;

namespace Examples.Web.Infrastructure;

public static class HttpsPolicyBuilderExtensions
{
    public static IApplicationBuilder UseHttpsRedirection(this IApplicationBuilder app, bool enabled)
    {
        if (enabled)
        {
            app.UseHttpsRedirection();
        }

        return app;
    }
}
