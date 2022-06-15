using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class MyActionFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public MyActionFilter(ILogger<MyActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogTrace("called.");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogTrace("called, Canceled={canceled}", context.Canceled);
        }


    }
}
