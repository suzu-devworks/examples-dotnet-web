using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Examples.WebApi.Applications.CQRS.Commands;
using Examples.WebApi.Applications.CQRS.Events;
using Examples.WebApi.Applications.CQRS.Models;
using MediatR;

namespace Examples.WebApi.Controllers
{
    // https://github.com/jbogard/MediatR/wiki

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CQRSController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<CQRSController> logger;

        public CQRSController(IMediator mediator, ILogger<CQRSController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("ping")]
        public async Task<ActionResult<Pong>> DoPing(string message)
        {
            // do handler 1, 2, 3.
            await mediator.Publish(new PingedEvent());

            var response = await mediator.Send(new PingCommand() { Message = message });

            return (response is not null) ? response : BadRequest();
        }

        [HttpGet("pingex")]
        public async Task<ActionResult<Pong>> DoPingExtended(string message = "hoge")
        {
            // do handler 3 only.
            await mediator.Publish(new PingedExtendEvent());

            // not found handler.
            var response = await mediator.Send(new PingExtendCommand() { Message = message });

            return (response is not null) ? response : BadRequest();
        }

        /// <summary>
        /// Exception Handler Example.
        /// </summary>
        /// <returns></returns>
        [HttpGet("thrown")]
        public async Task<IActionResult> DoException()
        {
            var response = await mediator.Send(new DoExceptionCommand());
            throw new ApplicationException("Exception Handled.");
        }

    }
}
