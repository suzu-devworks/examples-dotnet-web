using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;

namespace Examples.WebApi.Applications.CQRS.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
            => (_logger) = (logger);

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("Logging {request}.", typeof(TRequest).Name);
            var response = await next();
            _logger.LogInformation("Logging {request} to {response}.", typeof(TRequest).Name, typeof(TResponse).Name);

            return response;
        }

    }
}