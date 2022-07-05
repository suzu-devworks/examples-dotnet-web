using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Examples.WebApi.Applications.WeatherForecast.Models;
using Examples.WebApi.Infrastructure;

namespace Examples.WebApi.Controllers;

[ApiController]
[Route("api/v1/cors")]
[EnableCors(CorsPolicyDefines.SPA_POLICY_NAME)]
[AutoValidateAntiforgeryToken]
public class CrossOriginResourceController : ControllerBase
{
    private readonly ILogger<CrossOriginResourceController> _logger;

    public CrossOriginResourceController(ILogger<CrossOriginResourceController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Preflight Request.
    /// </summary>
    /// <returns>HTTP 204 No Content only.</returns>
    [HttpOptions]
    public IActionResult PreflightRequest()
    {
        _logger.LogTrace("called.");
        return NoContent();
    }

    /// <summary>
    /// Gets all Weather forecast items.
    /// </summary>
    /// <returns><see cref="WeatherForecast" /> items</returns>
    [HttpGet]
    [Produces("application/json")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogTrace("called.");

        var rng = new Random();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        })
        .ToArray();
    }

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet]
    [Route("antiforgery/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<WeatherForecast>> GetAsync(int id)
    {
        var token = HttpContext.Request.Cookies["X-XSRF-TOKEN"];

        _logger.LogTrace("called. [{id}]", id);
        _logger.LogTrace("token: [{token}]", token);

        if (Summaries.Length < id)
        {
            return NoContent();
        }

        var rng = new Random();
        var data = new WeatherForecast()
        {
            Date = DateTime.Now,
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[id]
        };

        await Task.CompletedTask;
        return Ok(data);
    }

    [HttpPost]
    [Route("antiforgery/{id}")]
    public async Task<IActionResult> UpdateAsync(int id, WeatherForecast data, [FromHeader(Name = "X-XSRF-TOKEN")] string? token)
    {
        _logger.LogTrace("called. [{id},{summary}]", id, data.Summary);
        _logger.LogTrace("token: [{token}]", token);

        await Task.CompletedTask;
        return NoContent();
    }

    [HttpDelete]
    [Route("antiforgery/{id}")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        _logger.LogTrace("called. [{id}]", id);

        await Task.CompletedTask;
        return NoContent();
    }

}
