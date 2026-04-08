using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingPageFilter(ILogger<LoggingPageFilter> logger) : IPageFilter
{
    private readonly ILogger<LoggingPageFilter> _logger = logger;

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingPageFilter), nameof(OnPageHandlerExecuted));
        _logger.LogTrace("{name}: called.", nameof(OnPageHandlerExecuted));
    }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingPageFilter), nameof(OnPageHandlerExecuting));
        _logger.LogTrace("{name}: called.", nameof(OnPageHandlerExecuting));
    }

    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingPageFilter), nameof(OnPageHandlerSelected));
        _logger.LogTrace("{name}: called.", nameof(OnPageHandlerSelected));
    }
}
