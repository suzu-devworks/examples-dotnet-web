using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.Infrastructure.Filters.ProcessingOrder.Pages;

public class LoggingAsyncPageFilter(ILogger<LoggingAsyncPageFilter> logger) : IAsyncPageFilter
{
    private readonly ILogger<LoggingAsyncPageFilter> _logger = logger;

    public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
    {
        _logger.LogTrace("{name}: called(before next).", nameof(OnPageHandlerExecutionAsync));

        var executed = await next();

        _logger.LogTrace("{name}: called(after next): Canceled={canceled}.", nameof(OnPageHandlerExecutionAsync), executed.Canceled);
    }

    public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
    {
        _logger.LogTrace("{name}: called.", nameof(OnPageHandlerSelectionAsync));

        return Task.CompletedTask;
    }
}