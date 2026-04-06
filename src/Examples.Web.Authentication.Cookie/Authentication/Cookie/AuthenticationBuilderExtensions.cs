using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Examples.Web.Authentication.Cookie;

/// <summary>
/// Extension methods for <see cref="AuthenticationBuilder" />.
/// </summary>
public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddCustomCookie(this AuthenticationBuilder builder
        , Action<CookieAuthenticationOptions>? configure = null)
    {
        builder.AddCookie(CookieDefaults.AuthenticationScheme,
            options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Forbidden/";
                options.LoginPath = "/Account/Login/";

                // Set the custom events type to our implementation.
                options.EventsType = typeof(CustomCookieAuthenticationEvents);

                configure?.Invoke(options);
            });

        // Add HttpContextAccessor to access HttpContext in views and other services.
        builder.Services.AddHttpContextAccessor();

        // Add the custom cookie authentication events to the DI container.
        builder.Services.AddScoped<CustomCookieAuthenticationEvents>();
        builder.Services.AddSingleton<IUserRepository, Repositories.InMemoryUserRepository>();

        return builder;
    }
}
