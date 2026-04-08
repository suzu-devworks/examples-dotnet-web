using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingAsyncPageFilter(ILogger<LoggingAsyncPageFilter> logger) : IAsyncPageFilter
{
    private readonly ILogger<LoggingAsyncPageFilter> _logger = logger;

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncPageFilter), "OnPageHandlerExecutionAsync.BeforeNext");
        _logger.ProcessingOrderCalledBeforeNext(nameof(OnPageHandlerExecutionAsync));

        var executed = await next();

        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncPageFilter), "OnPageHandlerExecutionAsync.AfterNext");
        _logger.ProcessingOrderCalledAfterNext(nameof(OnPageHandlerExecutionAsync), executed.Canceled);
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        FilterDiagnosticsTracker.Record(context.HttpContext, nameof(LoggingAsyncPageFilter), nameof(OnPageHandlerSelectionAsync));
        _logger.ProcessingOrderCalled(nameof(OnPageHandlerSelectionAsync));

        return Task.CompletedTask;
    }
}
