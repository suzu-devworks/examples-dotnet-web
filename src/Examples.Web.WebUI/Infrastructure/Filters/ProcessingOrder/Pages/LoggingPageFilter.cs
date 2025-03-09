using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.Infrastructure.Filters.ProcessingOrder.Pages;

public class LoggingPageFilter(ILogger<LoggingPageFilter> logger) : IPageFilter
{
    private readonly ILogger<LoggingPageFilter> _logger = logger;

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
        _logger.LogTrace("{name}: called.", nameof(OnPageHandlerExecuted));
    }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        _logger.LogTrace("{name}: called.", nameof(OnPageHandlerExecuting));
    }

    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
        _logger.LogTrace("{name}: called.", nameof(OnPageHandlerSelected));
    }
}