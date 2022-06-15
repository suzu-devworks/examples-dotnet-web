using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Examples.WebApi.Infrastructure.Extensions;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class CurlLoggingActionFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public CurlLoggingActionFilter(ILogger<CurlLoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("{command}", context.HttpContext.Request.ToCurlCommand());
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //no-operations.
        }

    }
}
