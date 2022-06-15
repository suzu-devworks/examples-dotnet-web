using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Examples.WebApi.Infrastructure.Filters
{
    public class AddHeaderResultServiceFilter : IResultFilter
    {
        private readonly ILogger _logger;
        public AddHeaderResultServiceFilter(ILogger<AddHeaderResultServiceFilter> logger)
        {
            _logger = logger;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var headerName = "OnResultExecuting";
            context.HttpContext.Response.Headers.Add(headerName, new string[] { "ResultExecutingSuccessfully" });
            _logger.LogInformation("Header added: {HeaderName}", headerName);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Can't add to headers here because response has started.
            _logger.LogInformation("AddHeaderResultServiceFilter.OnResultExecuted");
        }
    }
}
