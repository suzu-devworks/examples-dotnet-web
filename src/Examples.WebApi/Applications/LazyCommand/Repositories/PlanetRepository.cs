using System.Threading.Tasks;
using Examples.WebApi.Applications.LazyCommand.Models;

namespace Examples.WebApi.Applications.LazyCommand.Repositories
{
    public class PlanetRepository : IPlanetRepository
    {
        public async Task<Planet> GetPlanet(int planetId)
        {
            var planet = new Planet
            {
                PlanetId = planetId,
                Name = $"Planet-{planetId:D6}",
            };

            return await Task.FromResult(planet);
        }

    }
}