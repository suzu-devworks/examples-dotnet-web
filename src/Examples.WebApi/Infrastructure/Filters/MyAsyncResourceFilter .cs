using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class MyAsyncResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger _logger;

        public MyAsyncResourceFilter(ILogger<MyAsyncResourceFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            _logger.LogTrace("executing.");

            var executed = await next();

            _logger.LogTrace("executed, Canceled={canceled}.", executed.Canceled);
        }
    }
}
