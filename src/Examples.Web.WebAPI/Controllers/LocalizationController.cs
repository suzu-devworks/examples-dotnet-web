using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Examples.WebAPI.Applications.Localization;
using Examples.WebAPI.Applications.Localization.Models;

namespace Examples.Web.WebAPI.Controllers;

/// <summary>
/// Localization examples controller.
/// </summary>
/// <remarks>
/// <see href="https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/localization" >Globalization and localization in ASP.NET Core</see>
/// <example>
/// <code>
///     ?culture=ja
/// </code>
/// </example>
/// </remarks>
[ApiController]
[Route("[controller]")]
public class LocalizationController(ILogger<LocalizationController> logger,
    IStringLocalizer<LocalizationController> localizer,
    IStringLocalizer<SharedResource> sharedLocalizer,
    IStringLocalizerFactory factory) : ControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly IStringLocalizer<LocalizationController> _localizer = localizer;
    private readonly IStringLocalizer<SharedResource> _sharedLocalizer = sharedLocalizer;
    private readonly IStringLocalizer _factoryMadeLocalizer = factory.Create(typeof(SharedResource));

    [HttpGet("about")]
    public IActionResult GetAbout(
        [FromQuery(Name = "culture")] string? inputCulture,
        [FromQuery(Name = "ui-culture")] string? inputUiCulture)
    {
        _logger.LogDebug("CurrentCulture   = {culture} / {system}", inputCulture, CultureInfo.CurrentCulture);
        _logger.LogDebug("CurrentUICulture = {culture} / {system}", inputUiCulture, CultureInfo.CurrentUICulture);

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

    [HttpPost("annotations")]
    public IActionResult PostForAnnotations(
        [FromBody] RegisterData param,
        [FromQuery(Name = "culture")] string? inputCulture,
        [FromQuery(Name = "ui-culture")] string? inputUiCulture)
    {
        _logger.LogDebug("CurrentCulture   = {culture} / {system}", inputCulture, CultureInfo.CurrentCulture);
        _logger.LogDebug("CurrentUICulture = {culture} / {system}", inputUiCulture, CultureInfo.CurrentUICulture);

        return Ok(new
        {
            CurrentCulture = CultureInfo.CurrentCulture.ToString(),
            CurrentUICulture = CultureInfo.CurrentUICulture.ToString(),
            param.Email
        });
    }
}
