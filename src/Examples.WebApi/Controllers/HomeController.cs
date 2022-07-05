using Microsoft.AspNetCore.Mvc;

namespace Examples.WebApi.Controllers;

/// <summary>
/// A controller for redirecting root.
/// </summary>
[Route("/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : ControllerBase
{
    /// <summary>
    /// Redirects the route request.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index()
    {
        return new RedirectResult("~/swagger");
    }
}
