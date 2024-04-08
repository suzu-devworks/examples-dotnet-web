using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Examples.Web.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;

namespace Examples.Web.WebAPI.Controllers;

[EnableCors]
[ApiController]
[SwaggerTag("Security")]
[Route("[controller]")]
public class CorsResourceController(ILogger<CorsResourceController> logger) : ControllerBase
{
    private readonly ILogger<CorsResourceController> _logger = logger;

    /// <summary>
    /// Preflight Request.
    /// </summary>
    /// <response code="204">No content only</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    // [RequestHeaderParameter("origin", defaultValue: "https://foo.bar.org")]
    // [RequestHeaderParameter("access-control-request-method", defaultValue: "DELETE")]
    // [RequestHeaderParameter("access-control-request-headers", defaultValue: "Origin,X-Requested-With")]
    [HttpOptions]
    public IActionResult PreflightRequestAsync()
    {
        _logger.LogTrace("called.");

        //await Task.Delay(0, cancellationToken);
        return NoContent();
    }


    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        _logger.LogTrace("Get called.");

        var rng = new Random();
        var results = Enumerable.Range(0, 10)
            .Select(index => new
            {
                Index = index,
                Date = DateTimeOffset.Now.AddDays(index),
            });

        await Task.Delay(0, cancellationToken);
        return Ok(results);
    }


    [HttpPost]
    public async Task<IActionResult> PostAsync(IDictionary<string, string> inputs, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Post called.");
        inputs.Add("DateTimeOffset.Now", DateTimeOffset.Now.ToString());

        await Task.Delay(0, cancellationToken);
        return Ok(inputs);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(string id, IDictionary<string, string> inputs, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Put called by {id}.", id.Sanitize());
        inputs.Add("DateTimeOffset.Now", DateTimeOffset.Now.ToString());

        await Task.Delay(0, cancellationToken);
        return Ok(inputs);
    }


    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchAsync(string id, IDictionary<string, string> inputs, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Put called by {id}.", id.Sanitize());
        inputs.Add("DateTimeOffset.Now", DateTimeOffset.Now.ToString());

        await Task.Delay(0, cancellationToken);
        return Ok(inputs);
    }


    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Delete called by {id}. ", id.Sanitize());

        await Task.Delay(0, cancellationToken);
        return NoContent();
    }

}
