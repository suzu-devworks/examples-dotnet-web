using System.Linq;
using System.Text.RegularExpressions;

namespace Examples.Web.Infrastructure;

public static class WebStringExtensions
{
    public class SanitizeOptions
    {
    }

    public static string Sanitize(this string source, SanitizeOptions? _ = default)
    {
        return source.ControlCharEncode();
    }

    public static string ControlCharEncode(this string source)
        => ControlCharExpression.Replace(source, m => m.Value.HexDump(prefix: "%"));

    /// <summary>
    /// match control characters. 0x00(NUL) - 0x1F(US), 0x7F(DEL)
    /// </summary>
    private static readonly Regex ControlCharExpression = new(@"[\x00-\x1F\x7F]", RegexOptions.Compiled);

    public static string HexDump(this string source, string? separator = default, string? prefix = default)
        => string.Join(separator, source.Select(c => $"{prefix}{(uint)c:x2}"));

}
