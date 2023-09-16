using Microsoft.AspNetCore.Mvc;
using Examples.WebAPI.Models;

namespace Examples.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get Weather Forecast.
    /// </summary>
    /// <remarks>
    /// Remarks here.
    /// <code>
    /// var datas = Get();
    /// var date = DateTime.UtcNow;
    /// </code>
    /// </remarks>
    /// <returns></returns>
    /// <response code="200">Success:It's OK<br/></response>
    /// <response code="400">BadRequest:<br/>If the item is null</response>
    [HttpGet(Name = "GetWeatherForecast")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogTrace("called");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
