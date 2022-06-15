using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Examples.WebApi.Infrastructure.Startup;

/// <summary>
/// Extension methods for custom logging settings to an <see cref="ILoggingBuilder" />.
/// </summary>
/// <remarks>
/// <see href="https://nlog-project.org/config/" >Configuration options - NLog</see>
/// </remarks>
public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder UseCustomLogging(this ILoggingBuilder builder)
    {
        // clear default logging.
        builder.ClearProviders();
        builder.SetMinimumLevel(LogLevel.Trace);
        // delegate to NLog.
        builder.AddNLog();

        return builder;
    }
}
