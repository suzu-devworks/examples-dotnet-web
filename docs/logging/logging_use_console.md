# Logging using Microsoft.Extensions.Logging.Console

## Table of Contents <!-- omit in toc -->

- [Tiny colored console](#tiny-colored-console)
  - [Implemented in code](#implemented-in-code)
  - [You can do it just by setting it up](#you-can-do-it-just-by-setting-it-up)
- [The log is generated twice](#the-log-is-generated-twice)
- [References](#references)

## Tiny colored console

Can't we make the console logs a bit more colorful?

### Implemented in code

use `AddSimpleConsole()` in `Program.cs`:

```cs
builder.Logging.AddSimpleConsole(options =>
    {
        options.ColorBehavior = LoggerColorBehavior.Enabled;

        options.SingleLine = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    });
```

### You can do it just by setting it up

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
}
```

## The log is generated twice

If both console and debug targets are active, logs will be output to the VS Code debug console twice, so you need to disable one of them.

in `appsettings.json`:

```diff
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.AspNetCore.Hosting.Diagnostics": "Information"
+    },
+    "Debug": {
+      "LogLevel": {
+        "Default": "None"
    　　}
    }
  },
}
```

## References

- [Console log formatting - .NET | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/console-log-formatter)
