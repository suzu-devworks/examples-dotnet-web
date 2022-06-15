using System;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Examples.WebApi.Applications.Localization;
using Examples.WebApi.Applications.Localization.Models;

namespace Examples.WebApi.Controllers
{
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
    [Route("api/v1/[controller]")]
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

        [HttpGet("about")]
        public string GetAbout(
            [FromQuery(Name = "culture")] string? _,
            [FromQuery(Name = "ui-culture")] string? _1)
        {
            var title = _localizer["About Title"];
            var culture = _sharedLocalizer["Culture"];
            var timestamp = DateTime.Now.ToLongDateString();

            var message = $"{title} - {culture} - {timestamp}";
            _logger.LogInformation("CurrentCulture   = {culture}", CultureInfo.CurrentCulture);
            _logger.LogInformation("CurrentUICulture = {culture}", CultureInfo.CurrentUICulture);
            _logger.LogInformation("{message}", message);

            return message;
        }

        [HttpPost("annotations")]
        public IActionResult PostForAnnotations(
            [FromQuery(Name = "culture")] string? _,
            [FromQuery(Name = "ui-culture")] string? _1,
            [FromBody] RegisterData param)
        {
            _logger.LogInformation("CurrentCulture   = {culture}", CultureInfo.CurrentCulture);
            _logger.LogInformation("CurrentUICulture = {culture}", CultureInfo.CurrentUICulture);
            _logger.LogInformation(nameof(PostForAnnotations));

            return Ok();
        }
    }
}
