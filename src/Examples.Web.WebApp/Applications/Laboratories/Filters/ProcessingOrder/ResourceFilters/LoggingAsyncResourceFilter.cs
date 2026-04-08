using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingAsyncResourceFilter(ILogger<LoggingAsyncResourceFilter> logger) : IAsyncResourceFilter
{
    private readonly ILogger<LoggingAsyncResourceFilter> _logger = logger;

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncResourceFilter), "OnResourceExecutionAsync.BeforeNext");
        _logger.LogTrace("{name}: called(before next).", nameof(OnResourceExecutionAsync));

        var executed = await next();

        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncResourceFilter), "OnResourceExecutionAsync.AfterNext");
        _logger.LogTrace("{name}: called(after next): Canceled={canceled}.", nameof(OnResourceExecutionAsync), executed.Canceled);
    }
}
