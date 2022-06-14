using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR.Pipeline;

namespace Examples.WebApi.Applications.CQRS.Behaviors
{
    public class RequestPostProcessorBehaviorProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<RequestPostProcessorBehaviorProcessor<TRequest, TResponse>> _logger;

        public RequestPostProcessorBehaviorProcessor(ILogger<RequestPostProcessorBehaviorProcessor<TRequest, TResponse>> logger)
            => (_logger) = (logger);

        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processed {request} to {response}.", typeof(TRequest).Name, typeof(TResponse).Name);
            return Task.CompletedTask;
        }

    }
}