using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingAsyncExceptionFilter(ILogger<LoggingAsyncExceptionFilter> logger) : IAsyncExceptionFilter
{
    private readonly ILogger<LoggingAsyncExceptionFilter> _logger = logger;

    public Task OnExceptionAsync(ExceptionContext context)
    {
        _logger.LogTrace("{name}: called with {exception}.", nameof(OnExceptionAsync), context.Exception);

        return Task.CompletedTask;
    }
}
