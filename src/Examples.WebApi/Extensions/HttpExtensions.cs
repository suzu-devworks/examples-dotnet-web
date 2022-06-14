using System;
using Microsoft.AspNetCore.Http;

namespace Examples.WebApi.Extensions
{
    public static class HttpExtensions
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
}