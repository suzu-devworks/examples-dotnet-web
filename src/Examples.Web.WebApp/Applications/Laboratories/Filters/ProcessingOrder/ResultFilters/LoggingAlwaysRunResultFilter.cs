using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingAlwaysRunResultFilter(ILogger<LoggingAlwaysRunResultFilter> logger) : IAlwaysRunResultFilter
{
    private readonly ILogger<LoggingAlwaysRunResultFilter> _logger = logger;

    public void OnResultExecuting(ResultExecutingContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAlwaysRunResultFilter), nameof(OnResultExecuting));
        _logger.LogTrace("{name}: called.", nameof(OnResultExecuting));
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAlwaysRunResultFilter), nameof(OnResultExecuted));
        _logger.LogTrace("{name}: called: Canceled={canceled}", nameof(OnResultExecuted), context.Canceled);
    }
}
