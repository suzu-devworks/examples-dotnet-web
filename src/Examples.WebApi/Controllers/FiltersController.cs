using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Examples.WebApi.Infrastructure.Filters;

namespace Examples.WebApi.Controllers
{
    [AddHeaderResultFilter("Author", "suzu-devworks")]
    [Route("api/v1/[controller]")]
    public class FiltersController : Controller
    {
        private readonly ILogger<FiltersController> _logger;

        public FiltersController(ILogger<FiltersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(AddHeaderResultServiceFilter))]
        public IActionResult Index()
        {
            _logger.LogTrace($"{nameof(Index)}");
            return Ok();
        }

        [HttpGet("index2")]
        [ServiceFilter(typeof(MyActionFilterAttribute))]
        public IActionResult Index2(string param)
        {
            _logger.LogTrace("param={param}", param);
            return Content("Header values by ˚configuration.");
        }

        [HttpGet("index3")]
        [MyActionTypeFilter]
        [TypeFilter(typeof(MyActionTypeFilterAttribute))]
        [ServiceFilter(typeof(MyActionTypeFilterAttribute))]
        public IActionResult Index3()
        {
            _logger.LogTrace($"{nameof(Index3)}");
            return Content("From FilterTest.");
        }

        [HttpGet("some")]
        [ShortCircuitingResourceFilter]
        public IActionResult SomeResource()
        {
            _logger.LogTrace($"{nameof(SomeResource)}");
            return Content("Successful access to resource - header is set.");
        }

        [HttpGet("factory")]
        [AddHeaderWithFactory]
        public IActionResult HeaderWithFactory()
        {
            return Content("Examine the headers using the F12 developer tools.");
        }

        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

    }
}