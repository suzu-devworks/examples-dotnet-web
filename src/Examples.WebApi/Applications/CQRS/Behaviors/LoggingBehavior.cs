using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;

namespace Examples.WebApi.Applications.CQRS.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
            => (this.logger) = (logger);

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            logger.LogInformation($"Logging {typeof(TRequest).Name} handling.");
            var response = await next();
            logger.LogInformation($"Logging {typeof(TRequest).Name} handled to {typeof(TResponse).Name}.");

            return response;
        }

    }
}