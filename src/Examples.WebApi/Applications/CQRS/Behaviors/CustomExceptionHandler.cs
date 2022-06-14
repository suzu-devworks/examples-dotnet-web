using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Examples.WebApi.Applications.CQRS.Commands;
using MediatR.Pipeline;

namespace Examples.WebApi.Applications.CQRS.Behaviors
{
    public class CustomExceptionHandler : IRequestExceptionHandler<DoExceptionCommand, bool, Exception>
    {
        private readonly ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
            => (_logger) = (logger);

        public Task Handle(DoExceptionCommand request, Exception exception, RequestExceptionHandlerState<bool> state, CancellationToken cancellationToken)
        {
            var message = $"{this.GetType().Name} - {typeof(DoExceptionCommand).Name} - {exception.Message}";
            _logger.LogError("Handle! {message}", message);
            //state.SetHandled(false);
            return Task.CompletedTask;
        }

    }
}