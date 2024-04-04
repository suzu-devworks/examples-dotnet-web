using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.Infrastructure.Filters.ProcessingOrder.Resource;

public class LoggingAsyncResourceFilter(ILogger<LoggingAsyncResourceFilter> logger) : IAsyncResourceFilter
{
    private readonly ILogger<LoggingAsyncResourceFilter> _logger = logger;

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        _logger.LogTrace("{name}: called(before next).", nameof(OnResourceExecutionAsync));

        var executed = await next();

        _logger.LogTrace("{name}: called(after next): Canceled={canceled}.", nameof(OnResourceExecutionAsync), executed.Canceled);
    }
}
