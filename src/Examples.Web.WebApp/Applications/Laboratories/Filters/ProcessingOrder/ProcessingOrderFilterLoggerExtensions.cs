namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

internal static partial class ProcessingOrderFilterLoggerExtensions
{
    public static void ProcessingOrderCalled(this ILogger logger, string name)
        => LogCalled(logger, name);

    public static void ProcessingOrderCalledBeforeNext(this ILogger logger, string name)
        => LogCalledBeforeNext(logger, name);

    public static void ProcessingOrderCalledAfterNext(this ILogger logger, string name, bool canceled)
        => LogCalledAfterNext(logger, name, canceled);

    public static void ProcessingOrderCalledWithCanceled(this ILogger logger, string name, bool canceled)
        => LogCalledWithCanceled(logger, name, canceled);

    [LoggerMessage(EventId = 1000, Level = LogLevel.Trace, Message = "{name}: called.")]
    private static partial void LogCalled(ILogger logger, string name);

    [LoggerMessage(EventId = 1001, Level = LogLevel.Trace, Message = "{name}: called(before next).")]
    private static partial void LogCalledBeforeNext(ILogger logger, string name);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Trace, Message = "{name}: called(after next): Canceled={canceled}.")]
    private static partial void LogCalledAfterNext(ILogger logger, string name, bool canceled);

    [LoggerMessage(EventId = 1003, Level = LogLevel.Trace, Message = "{name}: called: Canceled={canceled}")]
    private static partial void LogCalledWithCanceled(ILogger logger, string name, bool canceled);
}
