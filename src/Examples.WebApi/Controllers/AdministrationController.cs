using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Examples.WebApi.Infrastructure;

namespace Examples.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[EnableCors(CorsPolicyDefines.SPA_POLICY_NAME)]
public class AdministrationController : ControllerBase
{
    private readonly ILogger<AdministrationController> _logger;
    private readonly IAntiforgery _antiforgery;

    public AdministrationController(ILogger<AdministrationController> logger, IAntiforgery antiforgery)
    {
        _logger = logger;
        _antiforgery = antiforgery;
    }


    [HttpGet]
    [Produces("application/json")]
    [Route("token")]
    [IgnoreAntiforgeryToken]
    public IActionResult GenerateAntiForgeryToken(bool includeCookie = true)
    {
        var token = _antiforgery.GetAndStoreTokens(HttpContext);

        if (includeCookie)
        {
            Response.Cookies.Append("XSRF-TOKEN", token?.RequestToken ?? "", new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                HttpOnly = false,
            });
            _logger.LogDebug(@"AntiForgery token append Response Cookie[""{name}""].", "XSRF-TOKEN");
        }

        return Ok(new Envelope { Token = token?.RequestToken });
    }

    private class Envelope
    {
        public string? Token { get; init; }
    }

}
