using Microsoft.AspNetCore.Mvc;

namespace Examples.Web.Authentication.Cookie.Extensions;

public static class UrlHelperExtensions
{
    public static string? GetLocalUrl(this IUrlHelper urlHelper, string localUrl)
    {
        if (!urlHelper.IsLocalUrl(localUrl))
        {
            return urlHelper.Page("/Index");
        }

        return localUrl;
    }
}
