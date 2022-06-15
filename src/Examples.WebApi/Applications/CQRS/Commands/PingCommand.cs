using Examples.WebApi.Applications.CQRS.Models;
using MediatR;

namespace Examples.WebApi.Applications.CQRS.Commands
{
    public class PingCommand : IRequest<Pong>
    {
        public string? Message { get; init; }
    }

    public class PingExtendCommand : PingCommand
    {
        // InvalidOperationException: Handler was not found for request of ...
    }

}