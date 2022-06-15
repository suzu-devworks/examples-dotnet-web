using System.Threading.Tasks;
using Examples.WebApi.Applications.LazyCommand.Models;

namespace Examples.WebApi.Applications.LazyCommand.Repositories
{
    public interface IPlanetRepository
    {
        Task<Planet> GetPlanet(int planetId);
    }
}
