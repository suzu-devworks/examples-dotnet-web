using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR.Pipeline;

namespace Examples.WebApi.Applications.CQRS.Behaviors
{
    public class RequestPostProcessorBehaviorProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<RequestPostProcessorBehaviorProcessor<TRequest, TResponse>> logger;

        public RequestPostProcessorBehaviorProcessor(ILogger<RequestPostProcessorBehaviorProcessor<TRequest, TResponse>> logger)
            => (this.logger) = (logger);

        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            logger.LogInformation($"processed {typeof(TRequest).Name} - {typeof(TResponse).Name}");
            return Task.CompletedTask;
        }

    }
}