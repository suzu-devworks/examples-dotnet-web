using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class MyActionTypeFilterAttribute : TypeFilterAttribute
    {
        public MyActionTypeFilterAttribute() : base(typeof(ActionFilterImpl))
        {
        }

        private class ActionFilterImpl : IActionFilter
        {
            private readonly ILogger _logger;

            public ActionFilterImpl(ILoggerFactory loggerFactory)
            {
                _logger = loggerFactory.CreateLogger<MyActionTypeFilterAttribute>();
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                _logger.LogInformation($"called.");
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                _logger.LogInformation($"called.");
            }
        }

    }
}
