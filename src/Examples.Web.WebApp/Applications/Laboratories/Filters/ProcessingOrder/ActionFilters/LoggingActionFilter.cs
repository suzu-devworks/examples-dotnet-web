using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingActionFilter(ILogger<LoggingActionFilter> logger) : IActionFilter
{
    private readonly ILogger<LoggingActionFilter> _logger = logger;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogTrace("{name}: called.", nameof(OnActionExecuting));
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogTrace("{name}: called: Canceled={canceled}", nameof(OnActionExecuted), context.Canceled);
    }
}
