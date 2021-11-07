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
        private readonly ILogger logger;
        private readonly IStringLocalizer<LocalizationController> localizer;
        private readonly IStringLocalizer<SharedResource> sharedLocalizer;
        private readonly IStringLocalizer factoryMadeLocalizer;

        public LocalizationController(ILogger<LocalizationController> logger,
            IStringLocalizer<LocalizationController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer,
            IStringLocalizerFactory factory)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
            this.factoryMadeLocalizer = factory.Create(typeof(SharedResource));
        }

        [HttpGet("about")]
        public string GetAbout(
            [FromQuery(Name = "culture")] string? _,
            [FromQuery(Name = "ui-culture")] string? _1)
        {
            var title = localizer["About Title"];
            var culture = sharedLocalizer["Culture"];
            var timestamp = DateTime.Now.ToLongDateString();

            var message = $"{title} - {culture} - {timestamp}";
            logger.LogInformation($"CurrentCulture   = {CultureInfo.CurrentCulture}");
            logger.LogInformation($"CurrentUICulture = {CultureInfo.CurrentUICulture}");
            logger.LogInformation(message);

            return message;
        }

        [HttpPost("annotations")]
        public IActionResult PostForAnnotations(
            [FromQuery(Name = "culture")] string? _,
            [FromQuery(Name = "ui-culture")] string? _1,
            [FromBody] RegisterData param)
        {
            logger.LogInformation($"CurrentCulture   = {CultureInfo.CurrentCulture}");
            logger.LogInformation($"CurrentUICulture = {CultureInfo.CurrentUICulture}");
            logger.LogInformation(nameof(PostForAnnotations));

            return Ok();
        }
    }
}