using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Examples.WebApi.Applications.LazyCommand.Repositories;

namespace Examples.WebApi.Applications.LazyCommand.Commands
{
    public class GetRocketCommand : IGetRocketCommand
    {
        private readonly IRocketRepository _rocketRepository;

        public GetRocketCommand(IRocketRepository rocketRepository) =>
            _rocketRepository = rocketRepository;

        public async Task<IActionResult> ExecuteAsync(int rocketId)
        {
            var rocket = await _rocketRepository.GetRocket(rocketId);
            return rocket is not null
                ? new OkObjectResult(rocket)
                : new NotFoundResult();
        }

    }
}
