using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class MyAsyncResultFilter : IAsyncResultFilter
    {
        private readonly ILogger logger;

        public MyAsyncResultFilter(ILogger<MyAsyncResultFilter> logger)
        {
            this.logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is not EmptyResult)
            {
                logger.LogTrace("executing.");

                var executed = await next();

                logger.LogTrace("executed, Canceled={canceled}.", executed.Canceled);
            }
            else
            {
                context.Cancel = true;
            }

        }
    }
}