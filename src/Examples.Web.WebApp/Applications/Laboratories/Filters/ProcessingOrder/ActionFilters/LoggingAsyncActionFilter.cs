using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingAsyncActionFilter(ILogger<LoggingAsyncActionFilter> logger) : IAsyncActionFilter
{
    private readonly ILogger<LoggingAsyncActionFilter> _logger = logger;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncActionFilter), "OnActionExecutionAsync.BeforeNext");
        _logger.LogTrace("{name}: called(before next).", nameof(OnActionExecutionAsync));

        var executed = await next();

        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncActionFilter), "OnActionExecutionAsync.AfterNext");
        _logger.LogTrace("{name}: called(after next): Canceled={canceled}.", nameof(OnActionExecutionAsync), executed.Canceled);
    }
}
