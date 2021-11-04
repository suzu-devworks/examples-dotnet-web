using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Examples.WebApi.Extensions
{
    static class HostBuilderLoggingExtensions
    {
        public static IHostBuilder ConfigureCustomLogging(this IHostBuilder builder)
            => builder.ConfigureCustomLogging(LoggingBuilderExtensions => { });

        public static IHostBuilder ConfigureCustomLogging(this IHostBuilder builder, Action<ILoggingBuilder> configureLogging)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                logging.AddNLog();

                configureLogging(logging);
            });

            return builder;
        }

    }
}