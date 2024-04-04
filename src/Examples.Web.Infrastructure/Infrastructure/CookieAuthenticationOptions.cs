using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Examples.Web.Infrastructure;

public static class CookieAuthenticationOptionsExtensions
{
    public static CookieAuthenticationOptions UseUnauthorizedApiHandler(this CookieAuthenticationOptions options)
    {
        var onRedirectToLogin = options.Events.OnRedirectToLogin;

        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Headers.Any(x => x.Key == HeaderNames.Accept && x.Value == MediaTypeNames.Application.Json))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }

            return onRedirectToLogin.Invoke(context);
        };

        return options;
    }

    public static CookieAuthenticationOptions UseForbiddenApiHandler(this CookieAuthenticationOptions options)
    {
        var onRedirectToAccessDenied = options.Events.OnRedirectToAccessDenied;

        options.Events.OnRedirectToAccessDenied = context =>
        {
            if (context.Request.Headers.Any(x => x.Key == HeaderNames.Accept && x.Value == MediaTypeNames.Application.Json))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }

            return onRedirectToAccessDenied.Invoke(context);
        };

        return options;
    }

}
