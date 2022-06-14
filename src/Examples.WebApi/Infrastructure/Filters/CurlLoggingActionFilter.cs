using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Examples.WebApi.Extensions;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class CurlLoggingActionFilter : IActionFilter
    {
        private readonly ILogger logger;

        public CurlLoggingActionFilter(ILogger<CurlLoggingActionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("{command}", context.HttpContext.Request.ToCurlCommand());
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //no-operations.
        }

    }
}
