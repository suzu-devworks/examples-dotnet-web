using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;

namespace Examples.WebApi.Applications.CQRS.Events
{
    public class PingedEventHandler1 : INotificationHandler<PingedEvent>
    {
        private readonly ILogger<PingedEventHandler1> _logger;

        public PingedEventHandler1(ILogger<PingedEventHandler1> logger)
            => (_logger) = (logger);

        public Task Handle(PingedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pong 1");
            return Task.CompletedTask;
        }

    }

    public class PingedEventHandler2 : INotificationHandler<PingedEvent>
    {
        private readonly ILogger<PingedEventHandler2> _logger;

        public PingedEventHandler2(ILogger<PingedEventHandler2> logger)
            => (_logger) = (logger);

        public Task Handle(PingedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pong 2");
            return Task.CompletedTask;
        }

    }

    public class PingedEventHandler3<TEvent> : INotificationHandler<TEvent>
        where TEvent : PingedEvent
    {
        private readonly ILogger<PingedEventHandler3<TEvent>> _logger;

        public PingedEventHandler3(ILogger<PingedEventHandler3<TEvent>> logger)
            => (_logger) = (logger);

        public Task Handle(TEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pong 3");
            return Task.CompletedTask;
        }

    }
    
}