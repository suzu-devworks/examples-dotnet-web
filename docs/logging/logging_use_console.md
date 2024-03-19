# Logging using Microsoft.Extensions.Logging.Console

## Table of Contents <!-- omit in toc -->

- [Logging using Microsoft.Extensions.Logging.Console](#logging-using-microsoftextensionsloggingconsole)
  - [References](#references)
  - [Tiny colored console](#tiny-colored-console)
## References

- [Console log formatting - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/console-log-formatter)


## Tiny colored console

use `AddSimpleConsole()`.

in `Program.cs`:

```cs
builder.Logging.AddSimpleConsole(options =>
    {
        options.ColorBehavior = LoggerColorBehavior.Enabled;

        options.SingleLine = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    });

```

or 

in `appsettings.json`:

```diff
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.AspNetCore.Hosting.Diagnostics": "Information"
+    },
+    "Console": {
+      "FormatterName": "simple",
+      "FormatterOptions": {
+        "SingleLine": true,
+        "TimestampFormat": "yyyy-MM-dd HH:mm:ss ",
+        "ColorBehavior": "Enabled"
      }
    }
  },
  "AllowedHosts": "*"
}
```
