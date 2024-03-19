using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.Infrastructure.Filters
{
    public class ShortCircuitingRazorMethodPageFilterAttribute(
            string[]? handlers = default
        ) : RazorPageMethodFilterAttribute(handlers)
    {
        protected override Task OnPageHandlerExecutionAsyncCore(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            // be short-circuited.
            context.Result = new ContentResult()
            {
                Content = "I short circuited."
            };

            // Don't call next (the ActionExecutionDelegate).
            //await next();

            return Task.CompletedTask;
        }
    }
}