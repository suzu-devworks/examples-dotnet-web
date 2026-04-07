using Examples.Web.Infrastructure.Filters;
using Examples.Web.WebApp.Applications.Laboratories.Filters;
using Examples.Web.WebApp.Applications.Laboratories.Filters.ProcessingOrder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Examples.Web.WebUI.Pages
{
    [ShortCircuitingResourceFilter(["ButtonX"])]
    [ShortCircuitingRazorMethodPageFilter(["Button3"])]
    [ServiceFilter<LoggingAsyncResultFilter>]
    [TypeFilter<LoggingAsyncPageFilter>]
    [ResponseHeader("Author", "suzuki")]
    public class Laboratory(ILogger<Laboratory> logger) : PageModel
    {
        private readonly ILogger<Laboratory> _logger = logger;

        public void OnGet()
        {
        }

        public Task OnPostButton1Async()
        {
            _logger.LogInformation("Razor button1 called");
            return Task.CompletedTask;
        }

        public Task OnPostButton2Async()
        {
            _logger.LogInformation("Razor button2 called");
            throw new ApplicationException("Throw an exception with button2");
        }

        public Task OnPostButton3Async()
        {
            _logger.LogInformation("Razor button3 called");
            return Task.CompletedTask;
        }
    }
}
