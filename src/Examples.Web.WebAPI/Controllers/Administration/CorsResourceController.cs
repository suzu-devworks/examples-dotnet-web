using Examples.Web.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Examples.Web.WebAPI.Controllers.Administration;

[EnableCors]
[SwaggerTag("Administration")]
[ApiController]
[Route("[controller]")]
public class CorsResourceController(ILogger<CorsResourceController> logger) : ControllerBase
{
    private readonly ILogger<CorsResourceController> _logger = logger;

    /// <summary>
    /// Preflight Request.
    /// </summary>
    /// <response code="204">No content only</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpOptions]
    public IActionResult PreflightRequest()
    {
        _logger.LogTrace("called.");

        return NoContent();
    }


    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogTrace("Get called.");

        var rng = new Random();
        var results = Enumerable.Range(0, 10)
            .Select(index => new
            {
                Index = index,
                Date = DateTimeOffset.Now.AddDays(index),
            });

        return Ok(results);
    }


    [HttpPost]
    public IActionResult Post(IDictionary<string, string> inputs)
    {
        _logger.LogTrace("Post called.");
        inputs.Add("DateTimeOffset.Now", DateTimeOffset.Now.ToString());

        return Ok(inputs);
    }


    [HttpPut("{id}")]
    public IActionResult Put(string id, IDictionary<string, string> inputs)
    {
        _logger.LogTrace("Put called by {id}.", id.Sanitize());
        inputs.Add("DateTimeOffset.Now", DateTimeOffset.Now.ToString());

        return Ok(inputs);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _logger.LogTrace("Delete called by {id}. ", id.Sanitize());

        return NoContent();
    }

}
