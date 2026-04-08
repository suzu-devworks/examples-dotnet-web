using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingResourceFilter(ILogger<LoggingResourceFilter> logger) : IResourceFilter
{
    private readonly ILogger<LoggingResourceFilter> _logger = logger;

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingResourceFilter), nameof(OnResourceExecuting));
        _logger.LogTrace("{name}: called.", nameof(OnResourceExecuting));
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingResourceFilter), nameof(OnResourceExecuted));
        _logger.LogTrace("{name}: called: Canceled={canceled}", nameof(OnResourceExecuted), context.Canceled);
    }

}
