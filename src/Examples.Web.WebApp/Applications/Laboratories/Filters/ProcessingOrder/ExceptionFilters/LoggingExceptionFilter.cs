using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;

public class LoggingExceptionFilter(ILogger<LoggingExceptionFilter> logger) : IExceptionFilter
{
    private readonly ILogger<LoggingExceptionFilter> _logger = logger;

    public void OnException(ExceptionContext context)
    {
        _logger.LogTrace("{name}: called with {exception}.", nameof(OnException), context.Exception);
    }
}
