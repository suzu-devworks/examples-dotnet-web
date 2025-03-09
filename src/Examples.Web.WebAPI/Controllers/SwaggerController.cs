using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Examples.Web.Infrastructure.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Examples.Web.WebAPI.Controllers;

/// <summary>
/// Controller for testing Swagger.
/// </summary>
[Authorize]
[ApiController]
[SwaggerTag("Configurations")]
[Route("[controller]")]
public class SwaggerController : ControllerBase
{
    /// <summary>
    /// Various swagger tests.
    /// </summary>
    /// <param name="param"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> IndexAsync(Parameter param, CancellationToken cancellationToken)
    {
        var results = param.HasRange
            ? Enumerable.Range(param.Start, param.End - param.Start)
                .Select(x => new { No = x })
            : [new { No = -1 }];

        await Task.Delay(0, cancellationToken);
        return Ok(results);
    }

    public record Parameter
    {
        /// <summary>
        /// Start number parameter.
        /// </summary>
        [Required]
        [DefaultValue(1)]
        [FromQuery(Name = "start")]
        public int Start { get; set; }

        /// <summary>
        /// End number parameter.
        /// </summary>
        [Required]
        [DefaultValue(10)]
        [FromQuery(Name = "end")]
        public int End { get; set; }

        [HideParameter]
        public bool HasRange => (Start > 0) && (End > 0);
    }

    /// <summary>
    /// Only here is AllowAnonymous.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "AllowAnonymous method.",
        Description = "This is a method with AllowAnonymous set to 200, but I can't get anything in particular.",
        OperationId = "IndexAnonymous"
    )]
    [HttpGet("anonymous")]
    public async Task<IActionResult> IndexAnonymousAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        return Ok();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    [HttpPost("echo")]
    public async Task<IActionResult> EchoAsync([FromBody] string message, CancellationToken cancellationToken)
    {
        await Task.Delay(0, cancellationToken);
        return Ok(new { message });
    }

}