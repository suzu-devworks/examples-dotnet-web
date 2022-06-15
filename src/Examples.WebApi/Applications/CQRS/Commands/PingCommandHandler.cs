using System.Threading;
using System.Threading.Tasks;

using Examples.WebApi.Applications.CQRS.Models;

using MediatR;

namespace Examples.WebApi.Applications.CQRS.Commands
{
    public class PingCommandHandler : IRequestHandler<PingCommand, Pong>
    {
        public Task<Pong> Handle(PingCommand request, CancellationToken cancellationToken)
        {
            var result = new Pong { Message = $"{request.Message} Pong" };
            return Task.FromResult(result);
        }
    }

}
