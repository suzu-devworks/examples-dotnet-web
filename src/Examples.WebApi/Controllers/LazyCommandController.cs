using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Examples.WebApi.Applications.LazyCommand.Commands;

namespace Examples.WebApi.Controllers
{
    /// <summary>
    /// Lazy command pattern examples controller.
    /// </summary>
    /// <remarks>
    /// <see href="https://rehansaeed.com/asp-net-core-lazy-command-pattern/" >ASP.NET Core Lazy Command Pattern - Muhammad Rehan Saeed</see>
    /// </remarks>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LazyCommandController : ControllerBase
    {
        private readonly ILogger<LazyCommandController> _logger;
        private readonly Lazy<IGetRocketCommand> _getRocketCommand;
        private readonly Lazy<ILaunchRocketCommand> _launchRocketCommand;

        public LazyCommandController(
            ILogger<LazyCommandController> logger,
            Lazy<IGetRocketCommand> getRocketCommand,
            Lazy<ILaunchRocketCommand> launchRocketCommand)
        {
            _logger = logger;
            _getRocketCommand = getRocketCommand;
            _launchRocketCommand = launchRocketCommand;
        }

        [HttpGet("{rocketId}")]
        public async Task<IActionResult> GetRocket(int rocketId)
        {
            _logger.LogTrace("called, [{rocketId}]", rocketId);
            return await _getRocketCommand.Value.ExecuteAsync(rocketId);
        }

        [HttpGet("{rocketId}/launch/{planetId}")]
        public async Task<IActionResult> LaunchRocket(int rocketId, int planetId)
        {
            _logger.LogTrace("called, [{rocketId}/launch/{planetId}]", rocketId, planetId);
            return await _launchRocketCommand.Value.ExecuteAsync(rocketId, planetId);
        }

    }
}