using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.Infrastructure.Filters.ProcessingOrder.Results;

public class LoggingAsyncAlwaysRunResultFilter : IAsyncAlwaysRunResultFilter
{
    private readonly ILogger<LoggingAsyncAlwaysRunResultFilter> _logger;

    public LoggingAsyncAlwaysRunResultFilter(ILogger<LoggingAsyncAlwaysRunResultFilter> logger) =>
        (_logger) = (logger);

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        _logger.LogTrace("{name}: called(before next).", nameof(OnResultExecutionAsync));

        var executed = await next();

        _logger.LogTrace("{name}: called(after next): Canceled={canceled}.", nameof(OnResultExecutionAsync), executed.Canceled);
    }
}
