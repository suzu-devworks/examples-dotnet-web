using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Examples.Web.WebAPI.Controllers;

/// <summary>
/// A controller for testing custom serializers.
/// </summary>
[SwaggerTag("Examples")]
[ApiController]
[Route("[controller]")]
public class SerializationController(ILogger<SerializationController> logger) : ControllerBase
{
    /// <summary>
    /// This action checks the serialization of data types.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<IEnumerable<Model>> Index()
    {
        var now = DateTimeOffset.Now;

        return new Model[] {
            new (1, "item-1"),
            new (2, "item-2", DateTime: now.DateTime, DateTimeOffset: now.ToUniversalTime(), TimeSpan: now.TimeOfDay, DayOfWeek: now.DayOfWeek),
            new (3, "item-3", LongValue: long.MaxValue, Range: new Range(4, 12), Index: new Index(7))
        };
    }

    /// <summary>
    /// This action checks the deserialization of data types.
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    /// <response code="204">Returns item for confirmation</response>
    /// <response code="400">If models is invalid</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public IActionResult Post(IEnumerable<Model> models)
    {
        foreach (var model in models)
        {
            logger.LogDebug("received: {model}", model);
        }

        return NoContent();
    }

    public record Model(
        int Id,
        string Name,
        DateTime? DateTime = null,
        DateTimeOffset? DateTimeOffset = null,
        TimeSpan? TimeSpan = null,
        DayOfWeek? DayOfWeek = null,
        long? LongValue = null,
        Range? Range = null,
        Index? Index = null
    )
    {
    }
}
