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

                configure?.Invoke(options);
            });

        //# Add HttpContextAccessor to access HttpContext in views and other services.
        builder.Services.AddHttpContextAccessor();

        return builder;
    }
}
