using System;

namespace Examples.Web.Infrastructure;

public static class WebStringExtensions
{
    public static string Sanitize(this string source, string replacement = @"¥n")
        => source.Replace(Environment.NewLine, replacement);

}
