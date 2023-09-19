namespace Examples.Web.Infrastructure;

public static class RequestLocalizationOptionsExtensions
{
    /// <summary>
    /// Sets the supported request time culture to a <see cref="RequestLocalizationOptions" />.
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <seealso href="https://github.com/dotnet/aspnetcore/blob/main/src/Middleware/Localization/src/QueryStringRequestCultureProvider.cs" />
    /// <seealso href="https://github.com/dotnet/aspnetcore/blob/main/src/Middleware/Localization/src/CookieRequestCultureProvider.cs" />
    /// <seealso href="https://github.com/dotnet/aspnetcore/blob/main/src/Middleware/Localization/src/AcceptLanguageHeaderRequestCultureProvider.cs" />
    public static RequestLocalizationOptions UseCustomCultures(this RequestLocalizationOptions options)
    {
        var supportedCultures = new[] { "ja", "ja-JP", "fr", "fr-CA", "en", "en-US" };
        if (supportedCultures.Length == 0)
        {
            throw new ArgumentException($"{nameof(supportedCultures)} is empty", nameof(supportedCultures));
        }

        // options.RequestCultureProviders;
        // [0] QueryStringRequestCultureProvider
        // [1] CookieRequestCultureProvider
        // [2] AcceptLanguageHeaderRequestCultureProvider

        options.SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        // Add Header [Content-Language]
        options.ApplyCurrentCultureToResponseHeaders = true;

        return options;
    }
}
