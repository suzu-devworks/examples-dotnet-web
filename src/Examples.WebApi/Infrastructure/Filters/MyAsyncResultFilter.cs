using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class MyAsyncResultFilter : IAsyncResultFilter
    {
        private readonly ILogger _logger;

        public MyAsyncResultFilter(ILogger<MyAsyncResultFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is not EmptyResult)
            {
                _logger.LogTrace("executing.");

                var executed = await next();

                _logger.LogTrace("executed, Canceled={canceled}.", executed.Canceled);
            }
            else
            {
                context.Cancel = true;
            }

        }
    }
}
