using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Examples.WebApi.Applications.CQRS.Commands;
using MediatR.Pipeline;

using ExceptionType = System.ApplicationException;

namespace Examples.WebApi.Applications.CQRS.Behaviors
{
    public class CustomExceptionActionByImplement1 : IRequestExceptionAction<DoExceptionCommand, ExceptionType>
    {
        private readonly ILogger<CustomExceptionActionByImplement1> _logger;

        public CustomExceptionActionByImplement1(ILogger<CustomExceptionActionByImplement1> logger)
            => (_logger) = (logger);

        public Task Execute(DoExceptionCommand request, ExceptionType exception, CancellationToken cancellationToken)
        {
            _logger.LogError($"Action! {this.GetType().Name} - {typeof(DoExceptionCommand).Name} - {exception.Message}");
            return Task.CompletedTask;
        }

    }


    public class CustomExceptionActionByImplement2 : IRequestExceptionAction<DoExceptionCommand>
    {
        private readonly ILogger<CustomExceptionActionByImplement2> _logger;

        public CustomExceptionActionByImplement2(ILogger<CustomExceptionActionByImplement2> logger)
            => (_logger) = (logger);

        public Task Execute(DoExceptionCommand request, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError($"Action! {this.GetType().Name} - {typeof(DoExceptionCommand).Name} - {exception.Message}");
            return Task.CompletedTask;
        }

    }


    public class CustomExceptionActionByInheritance1 : RequestExceptionAction<DoExceptionCommand, ExceptionType>
    {
        private readonly ILogger<CustomExceptionActionByInheritance1> _logger;

        public CustomExceptionActionByInheritance1(ILogger<CustomExceptionActionByInheritance1> logger)
            => (_logger) = (logger);

        protected override void Execute(DoExceptionCommand request, ExceptionType exception)
        {
            _logger.LogError($"Action! {this.GetType().Name} - {typeof(DoExceptionCommand).Name} - {exception.Message}");
        }

    }


    public class CustomExceptionActionByInheritance2 : RequestExceptionAction<DoExceptionCommand>
    {
        private readonly ILogger<CustomExceptionActionByInheritance2> _logger;

        public CustomExceptionActionByInheritance2(ILogger<CustomExceptionActionByInheritance2> logger)
            => (_logger) = (logger);

        protected override void Execute(DoExceptionCommand request, Exception exception)
        {
            _logger.LogError($"Action! {this.GetType().Name} - {typeof(DoExceptionCommand).Name} - {exception.Message}");
        }

    }


    public class CustomExceptionActionByInheritance3 : AsyncRequestExceptionAction<DoExceptionCommand>
    {
        private readonly ILogger<CustomExceptionActionByInheritance3> _logger;

        public CustomExceptionActionByInheritance3(ILogger<CustomExceptionActionByInheritance3> logger)
            => (_logger) = (logger);

        protected override Task Execute(DoExceptionCommand request, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError($"Action! {this.GetType().Name} - {typeof(DoExceptionCommand).Name} - {exception.Message}");
            return Task.CompletedTask;
        }

    }

}