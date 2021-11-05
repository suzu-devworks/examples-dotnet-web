using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Examples.WebApi.Applications.LazyCommand.Repositories;

namespace Examples.WebApi.Applications.LazyCommand.Commands
{
    public class LaunchRocketCommand : ILaunchRocketCommand
    {
        private readonly IPlanetRepository _planetRepository;
        private readonly IRocketRepository _rocketRepository;

        public LaunchRocketCommand(
            IPlanetRepository planetRepository,
            IRocketRepository rocketRepository)
        {
            _planetRepository = planetRepository;
            _rocketRepository = rocketRepository;
        }

        public async Task<IActionResult> ExecuteAsync(int rocketId, int planetId)
        {
            var rocket = await _rocketRepository.GetRocket(rocketId);
            if (rocket == null)
            {
                return new NotFoundResult();
            }

            var planet = await _planetRepository.GetPlanet(planetId);
            if (planet == null)
            {
                return new NotFoundResult();
            }

            _rocketRepository.VisitPlanet(rocket, planet);
            return new OkObjectResult(rocket);
        }
    }
}