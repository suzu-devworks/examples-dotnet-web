using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class MyAsyncActionFilter : IAsyncActionFilter
    {
        private readonly ILogger _logger;

        public MyAsyncActionFilter(ILogger<MyAsyncActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogTrace("executing.");

            var executed = await next();

            _logger.LogTrace("executed. Canceled={canceled}", executed.Canceled);
        }
    }
}
