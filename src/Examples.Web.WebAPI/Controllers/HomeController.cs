using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Examples.Web.WebAPI.Controllers;

/// <summary>
/// A controller for redirecting root.
/// </summary>
[AllowAnonymous]
[IgnoreAntiforgeryToken]
[ApiExplorerSettings(IgnoreApi = true)]
[Route("/")]
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
