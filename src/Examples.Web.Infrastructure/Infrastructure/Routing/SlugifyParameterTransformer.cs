using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace Examples.Web.Infrastructure.Routing;

/// <summary>
/// Define a contract to convert to kebab-case URI.
/// </summary>
/// <seealso href="https://learn.microsoft.com/ja-jp/aspnet/core/mvc/controllers/routing?view=aspnetcore-8.0#use-a-parameter-transformer-to-customize-token-replacement"/> 
public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value is null) { return null; }

        return Regex.Replace(value.ToString()!,
                        "([a-z])([A-Z])",
                        "$1-$2",
                        RegexOptions.CultureInvariant,
                        TimeSpan.FromMilliseconds(100)
                    )
                    .ToLowerInvariant();
    }
}
