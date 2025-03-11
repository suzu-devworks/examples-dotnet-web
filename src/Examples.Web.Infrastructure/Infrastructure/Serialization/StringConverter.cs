using System;
using System.Text.RegularExpressions;

namespace Examples.Web.Infrastructure.Serialization;

public static partial class StringConverter
{
    public static Index ToIndex(string? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var match = IndexExpression.Match(value);
        if (!match.Success)
        {
            throw new ArgumentException($"Illegal value is [{value}].");
        }

        var fromEnd = match.Groups[1].Value == "^";
        var converted = int.Parse(match.Groups[2].Value);

        return new(converted, fromEnd);
    }

    public static Range ToRange(string? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var indexes = value.Split("..");
        if (indexes is not { Length: 2 })
        {
            throw new ArgumentException($"Illegal value is [{value}].");
        }

        var start = ToIndex(indexes[0]);
        var end = ToIndex(indexes[1]);

        return new(start, end);
    }

#if NET7_0_OR_GREATER
    private static readonly Regex IndexExpression = IndexExpressionGenerator();

    [GeneratedRegex("^(\\^?)(\\d{1,9})$", RegexOptions.Compiled)]
    private static partial Regex IndexExpressionGenerator();

#else
    private static readonly Regex IndexExpression = new(@"^(\\^?)(\\d{1,9})$", RegexOptions.Compiled);

#endif

}