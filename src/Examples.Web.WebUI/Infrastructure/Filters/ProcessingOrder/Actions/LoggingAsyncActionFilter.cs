using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.Infrastructure.Filters.ProcessingOrder.Actions;

public class LoggingAsyncActionFilter(ILogger<LoggingAsyncActionFilter> logger) : IAsyncActionFilter
{
    private readonly ILogger<LoggingAsyncActionFilter> _logger = logger;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogTrace("{name}: called(before next).", nameof(OnActionExecutionAsync));

        var executed = await next();

        _logger.LogTrace("{name}: called(after next): Canceled={canceled}.", nameof(OnActionExecutionAsync), executed.Canceled);
    }

}
