using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingAsyncPageFilter(ILogger<LoggingAsyncPageFilter> logger) : IAsyncPageFilter
{
    private readonly ILogger<LoggingAsyncPageFilter> _logger = logger;

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncPageFilter), "OnPageHandlerExecutionAsync.BeforeNext");
        _logger.LogTrace("{name}: called(before next).", nameof(OnPageHandlerExecutionAsync));

        var executed = await next();

        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncPageFilter), "OnPageHandlerExecutionAsync.AfterNext");
        _logger.LogTrace("{name}: called(after next): Canceled={canceled}.", nameof(OnPageHandlerExecutionAsync), executed.Canceled);
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncPageFilter), nameof(OnPageHandlerSelectionAsync));
        _logger.LogTrace("{name}: called.", nameof(OnPageHandlerSelectionAsync));

        return Task.CompletedTask;
    }
}
