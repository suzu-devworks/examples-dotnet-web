using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR.Pipeline;

namespace Examples.WebApi.Applications.CQRS.Behaviors
{
    public class RequestPreProcessorBehaviorProcessor<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        private readonly ILogger<RequestPreProcessorBehaviorProcessor<TRequest>> _logger;

        public RequestPreProcessorBehaviorProcessor(ILogger<RequestPreProcessorBehaviorProcessor<TRequest>> logger)
            => (_logger) = (logger);

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Pre-Process {typeof(TRequest).Name}");
            return Task.CompletedTask;
        }
        
    }
}