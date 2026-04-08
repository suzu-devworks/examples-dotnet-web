using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingAsyncResultFilter(ILogger<LoggingAsyncResultFilter> logger) : IAsyncResultFilter
{
    private readonly ILogger<LoggingAsyncResultFilter> _logger = logger;

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncResultFilter), "OnResultExecutionAsync.BeforeNext");
        _logger.LogTrace("{name}: called(before next).", nameof(OnResultExecutionAsync));

        var executed = await next();

        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncResultFilter), "OnResultExecutionAsync.AfterNext");
        _logger.LogTrace("{name}: called(after next): Canceled={canceled}.", nameof(OnResultExecutionAsync), executed.Canceled);
    }
}
