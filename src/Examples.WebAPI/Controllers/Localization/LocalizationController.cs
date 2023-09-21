using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Examples.Web.Infrastructure;
using Examples.Web.Infrastructure.Swagger;
using Examples.WebAPI.Applications.Localization;
using Examples.WebAPI.Applications.Localization.Models;

namespace Examples.WebAPI.Controllers.Localization;

/// <summary>
/// Localization examples controller.
/// </summary>
/// <remarks>
/// <see href="https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/localization" >Globalization and localization in ASP.NET Core</see>
/// <example>
/// <code>
///     ?culture=fr
/// </code>
/// </example>
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class LocalizationController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IStringLocalizer<LocalizationController> _localizer;
    private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
    private readonly IStringLocalizer _factoryMadeLocalizer;

    public LocalizationController(ILogger<LocalizationController> logger,
        IStringLocalizer<LocalizationController> localizer,
        IStringLocalizer<SharedResource> sharedLocalizer,
        IStringLocalizerFactory factory)
    {
        _logger = logger;
        _localizer = localizer;
        _sharedLocalizer = sharedLocalizer;
        _factoryMadeLocalizer = factory.Create(typeof(SharedResource));
    }

    [RequestHeaderParameterAppenderAttribute(name: "Accept-Language", defaultValue: "ja")]
    [HttpGet("about")]
    public IActionResult GetAbout(
        [FromQuery(Name = "culture")] string? inputCulture,
        [FromQuery(Name = "ui-culture")] string? inputUiCulture)
    {
        _logger.LogDebug("CurrentCulture   = {culture} / {system}",
             inputCulture?.Sanitize(),
             CultureInfo.CurrentCulture);
        _logger.LogDebug("CurrentUICulture = {culture} / {system}",
            inputUiCulture?.Sanitize(),
            CultureInfo.CurrentUICulture);

        var title = _localizer["About Title"];
        var culture = _sharedLocalizer["Culture"];
        var message = _factoryMadeLocalizer["Message"];
        var timestamp = DateTime.Now.ToLongDateString();

        return Ok(new
        {
            CurrentCulture = CultureInfo.CurrentCulture.ToString(),
            CurrentUICulture = CultureInfo.CurrentUICulture.ToString(),
            title,
            culture,
            message,
            timestamp
        });
    }

    [RequestHeaderParameterAppenderAttribute(name: "Accept-Language", defaultValue: "ja")]
    [HttpPost("annotations")]
    public IActionResult PostForAnnotations(
        [FromBody] RegisterData param,
        [FromQuery(Name = "culture")] string? inputCulture,
        [FromQuery(Name = "ui-culture")] string? inputUiCulture)
    {
        _logger.LogDebug("CurrentCulture   = {culture} / {system}",
             inputCulture?.Sanitize(),
             CultureInfo.CurrentCulture);
        _logger.LogDebug("CurrentUICulture = {culture} / {system}",
            inputUiCulture?.Sanitize(),
            CultureInfo.CurrentUICulture);

        return Ok(new
        {
            CurrentCulture = CultureInfo.CurrentCulture.ToString(),
            CurrentUICulture = CultureInfo.CurrentUICulture.ToString(),
            param.Email
        });
    }
}
