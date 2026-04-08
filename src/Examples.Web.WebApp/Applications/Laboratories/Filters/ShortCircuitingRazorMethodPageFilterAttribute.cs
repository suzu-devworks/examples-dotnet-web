using Examples.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Examples.Web.WebApp.Applications.Laboratories.Filters
{
    public class ShortCircuitingRazorMethodPageFilterAttribute(
            string[]? handlers = default
        ) : RazorPageMethodFilterAttribute(handlers)
    {
        protected override Task OnPageHandlerSelectionAsyncCore(PageHandlerSelectedContext context)
        {
            FilterDiagnosticsTracker.Record(context.HttpContext, nameof(ShortCircuitingRazorMethodPageFilterAttribute), nameof(OnPageHandlerSelectionAsyncCore));
            return Task.CompletedTask;
        }

        protected override Task OnPageHandlerExecutionAsyncCore(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            FilterDiagnosticsTracker.Record(context.HttpContext, nameof(ShortCircuitingRazorMethodPageFilterAttribute), nameof(OnPageHandlerExecutionAsyncCore));

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
