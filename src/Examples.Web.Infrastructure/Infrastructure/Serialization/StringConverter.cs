using System.Text.RegularExpressions;

namespace Examples.Web.Infrastructure.Serialization;

public static partial class StringConverter
{

#if NET7_0_OR_GREATER
    [GeneratedRegex("^(\\^?)(\\d{1,9})$", RegexOptions.Compiled)]
    private static partial Regex IndexExpressionGenerator();
    private static readonly Regex IndexExpression = IndexExpressionGenerator();

#else
    private static readonly Regex IndexExpression = new Regex(@"^(\\^?)(\\d{1,9})$", RegexOptions.Compiled);

#endif

    public static Index ToIndex(string? value)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));

        var match = IndexExpression.Match(value);
        if (!match.Success)
        {
            throw new ArgumentException($"Illigal value is [{value}].");
        }

        var fromEnd = match.Groups[1].Value == "^";
        var converted = int.Parse(match.Groups[2].Value);

        return new(converted, fromEnd);
    }

    public static Range ToRange(string? value)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));

        var indexes = value.Split("..");
        if (indexes is not { Length: 2 })
        {
            throw new ArgumentException($"Illigal value is [{value}].");
        }

        var start = ToIndex(indexes[0]);
        var end = ToIndex(indexes[1]);

        return new(start, end);
    }

}
