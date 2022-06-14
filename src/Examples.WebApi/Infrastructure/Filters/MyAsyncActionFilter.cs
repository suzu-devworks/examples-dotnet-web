using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class MyAsyncActionFilter : IAsyncActionFilter
    {
        private readonly ILogger logger;

        public MyAsyncActionFilter(ILogger<MyAsyncActionFilter> logger)
        {
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            logger.LogTrace("executing.");

            var executed = await next();
            
            logger.LogTrace("executed. Canceled={canceled}", executed.Canceled);
        }
    }
}