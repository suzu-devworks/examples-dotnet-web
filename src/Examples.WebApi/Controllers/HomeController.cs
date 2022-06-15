using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1822 // Member 'xxx' does not access instance data and can be marked as static

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
