using Microsoft.AspNetCore.Mvc;
using Examples.Web.Infrastructure.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Examples.Web.WebAPI.Controllers;

/// <summary>
/// A controller for testing custom serializers.
/// </summary>
[ApiController]
[SwaggerTag("Examples")]
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
            new (3, "item-3", LongValue: long.MaxValue, Range: new Range(4, 12), Index: new Index(7)),
            new (4, "item-4", LocalDateTime: DateTime.Now, UtcDateTime: DateTime.Now, UnspecifiedDateTime: DateTime.Now ),
            new (5, "item-5", LocalDateTime: DateTime.UtcNow, UtcDateTime: DateTime.UtcNow, UnspecifiedDateTime: DateTime.UtcNow ),
        };
    }

    /// <summary>
    /// This action checks the deserialization of data types.
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
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
        [property: ZoneHandlingDateTimeJsonConverter(kind: DateTimeKind.Local)] DateTime? LocalDateTime = null,
        [property: ZoneHandlingDateTimeJsonConverter(kind: DateTimeKind.Utc)] DateTime? UtcDateTime = null,
        [property: ZoneHandlingDateTimeJsonConverter(kind: DateTimeKind.Unspecified)] DateTime? UnspecifiedDateTime = null,
        long? LongValue = null,
        Range? Range = null,
        Index? Index = null
    );

}
