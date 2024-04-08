# Code scanning / CodeQL

## Table of Contents <!-- omit in toc -->

- [Code scanning / CodeQL](#code-scanning--codeql)
  - [Tips](#tips)
    - [Log entries created from user input (High)](#log-entries-created-from-user-input-high)

## Tips 

### Log entries created from user input (High)

If unsanitized user input is written to a log entry, a malicious user may be able to forge new log entries.

- OWASP: [Log Injection](https://owasp.org/www-community/attacks/Log_Injection).
- Common Weakness Enumeration: [CWE-117](https://cwe.mitre.org/data/definitions/117.html).

Looking at the explanation, it seems like removing the line breaks is a good idea, but that's not the case; there is a risk of login injection if the user input values are logged as they are, so I think we need to think of some more countermeasures.

```cs
    String username = ctx.Request.QueryString["username"];
    // BAD: User input logged as-is
    logger.Warn(username + " log in requested.");
    // GOOD: User input logged with new-lines removed
    logger.Warn(username.Replace(Environment.NewLine, "") + " log in requested");
```

When researching sanitization methods, I found the following methods:

- use [HttpUtility.HtmlEncode](https://learn.microsoft.com/ja-jp/dotnet/api/system.web.httputility.htmlencode)
- use [HtmlSanitizer NuGet package](https://www.nuget.org/packages/HtmlSanitizer/)

First, let's call a utility method (extension method) and then think about it.

```cs
    public class SanitizeOptions
    {
    }

    public static string Sanitize(this string source, SanitizeOptions? _ = default)
    {
        return source.ControlCharEncode();
    }

    public static string ControlCharEncode(this string source)
        => ControlCharExpression.Replace(source,
            m => string.Join("", m.Value.Select(c => $"%{(uint)c:x2}")));

    /// <summary>
    /// match control characters. 0x00(NUL) - 0x1F(US), 0x7F(DEL)
    /// </summary>
    private static readonly Regex ControlCharExpression = new(@"[\x00-\x1F\x7F]", RegexOptions.Compiled);
```

Next, escape the potentially malicious control code. `HttpUtility.UrlEncode` could also be used, but I just want to convert it to HEX characters, so I wrote it in code.

We will only add options such as `HttpUtility.HtmlEncode` if the conversion requirements increase.
