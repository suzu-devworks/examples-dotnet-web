using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Examples.Web.WebAPI.Controllers;

[ApiController]
[SwaggerTag("Configurations")]
[Route("[controller]")]
public class LoggingController : ControllerBase
{
    private readonly ILogger<LoggingController> _logger;

    public LoggingController(ILogger<LoggingController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        _logger.LogTrace("Trace logged.");
        _logger.LogDebug("Debug logged.");
        _logger.LogInformation("Information logged.");
        _logger.LogWarning("Warning logged.");
        _logger.LogError("Error logged.");
        _logger.LogCritical("Critical logged.");

        return Ok();
    }
}