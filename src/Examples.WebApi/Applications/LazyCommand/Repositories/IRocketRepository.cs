using System.Threading.Tasks;
using Examples.WebApi.Applications.LazyCommand.Models;

namespace Examples.WebApi.Applications.LazyCommand.Repositories
{
    public interface IRocketRepository
    {
        Task<Rocket> GetRocket(int rocketId);

        void VisitPlanet(Rocket rocket, Planet planet);

    }
}