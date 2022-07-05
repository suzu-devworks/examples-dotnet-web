using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

namespace Examples.WebApi.Infrastructure.Startup;

/// <summary>
/// Extension methods for custom localization settings to a <see cref="RequestLocalizationMiddleware" />.
/// </summary>
public static class LocalizationApplicationBuilderExtensions
{
    /// <summary>
    /// Sets the supported request time culture to a <see cref="RequestLocalizationMiddleware" />.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = new[] { "ja", "fr", "fr-CA", "en", "en-US" };

        app.UseRequestLocalization(options =>
        {
            options
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures)
                .SetDefaultCulture(supportedCultures[0]);

            // Add Header [Content-Language]
            options.ApplyCurrentCultureToResponseHeaders = true;
        });

        return app;
    }
}
