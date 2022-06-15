using System;
using Microsoft.AspNetCore.Http;

namespace Examples.WebApi.Infrastructure.Extensions;

/// <summary>
/// Extension methods for get the information contained in the <see cref="HttpRequest" />.
/// </summary>
public static class HttpRequestExtensions
{
    public static Uri ToUri(this HttpRequest request)
    {
        var uri = new UriBuilder(
            request.Scheme,
            request.Host.Host,
            request.Host.Port!.Value,
            request.Path,
            request.QueryString.Value
        ).Uri;

        return uri;
    }

    public static string ToCurlCommand(this HttpRequest request)
        => $"curl -X '{request.Method}' {request.ToUri()}";

}
