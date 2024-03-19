using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.Infrastructure.Filters.ProcessingOrder.Results;

public class LoggingAlwaysRunResultFilter(ILogger<LoggingAlwaysRunResultFilter> logger) : IAlwaysRunResultFilter
{
    private readonly ILogger<LoggingAlwaysRunResultFilter> _logger = logger;

    public void OnResultExecuting(ResultExecutingContext context)
    {
        _logger.LogTrace("{name}: called.", nameof(OnResultExecuting));
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        _logger.LogTrace("{name}: called: Canceled={canceled}", nameof(OnResultExecuted), context.Canceled);
    }

}
