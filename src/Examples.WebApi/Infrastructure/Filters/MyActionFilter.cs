using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class MyActionFilter : IActionFilter
    {
        private readonly ILogger logger;

        public MyActionFilter(ILogger<MyActionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogTrace("called.");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogTrace("called, Canceled={canceled}", context.Canceled);
        }


    }
}