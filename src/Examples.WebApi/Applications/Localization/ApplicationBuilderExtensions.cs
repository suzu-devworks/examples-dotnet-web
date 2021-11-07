using Microsoft.AspNetCore.Builder;

namespace Examples.WebApi.Applications.Localization
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomRequestLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new[] { "ja", "fr", "fr-CA", "en", "en-US" };

            app.UseRequestLocalization(options =>
            {
                // Set request time cultures.
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
}