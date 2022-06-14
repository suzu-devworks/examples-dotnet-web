using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class MyAsyncResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger logger;

        public MyAsyncResourceFilter(ILogger<MyAsyncResourceFilter> logger)
        {
            this.logger = logger;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            logger.LogTrace("executing.");

            var executed = await next();
            
            logger.LogTrace("executed, Canceled={canceled}.", executed.Canceled);
        }
    }
}