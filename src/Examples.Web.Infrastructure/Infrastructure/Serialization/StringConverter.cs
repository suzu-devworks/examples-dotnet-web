using System;
using System.Text.RegularExpressions;

namespace Examples.Web.Infrastructure.Serialization;

public static partial class StringConverter
{
    private static readonly Regex IndexExpression = MyRegex();

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

    [GeneratedRegex(@"^(\^?)(\d{1,9})$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}
