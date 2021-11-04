using Microsoft.AspNetCore.Mvc;

namespace Examples.WebApi.Controllers
{
    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}