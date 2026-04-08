using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingActionFilter(ILogger<LoggingActionFilter> logger) : IActionFilter
{
    private readonly ILogger<LoggingActionFilter> _logger = logger;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingActionFilter), nameof(OnActionExecuting));
        _logger.ProcessingOrderCalled(nameof(OnActionExecuting));
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingActionFilter), nameof(OnActionExecuted));
        _logger.ProcessingOrderCalledWithCanceled(nameof(OnActionExecuted), context.Canceled);
    }
}
