using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingResultFilter(ILogger<LoggingResultFilter> logger) : IResultFilter
{
    private readonly ILogger<LoggingResultFilter> _logger = logger;

    public void OnResultExecuting(ResultExecutingContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingResultFilter), nameof(OnResultExecuting));
        _logger.LogTrace("{name}: called.", nameof(OnResultExecuting));
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingResultFilter), nameof(OnResultExecuted));
        _logger.LogTrace("{name}: called: Canceled={canceled}", nameof(OnResultExecuted), context.Canceled);
    }
}
