using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.Infrastructure.Filters.ProcessingOrder.Resource;

public class LoggingResourceFilter(ILogger<LoggingResourceFilter> logger) : IResourceFilter
{
    private readonly ILogger<LoggingResourceFilter> _logger = logger;

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        _logger.LogTrace("{name}: called.", nameof(OnResourceExecuting));
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        _logger.LogTrace("{name}: called: Canceled={canceled}", nameof(OnResourceExecuted), context.Canceled);
    }

}
