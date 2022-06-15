using MediatR;

namespace Examples.WebApi.Applications.CQRS.Commands
{
    public class DoExceptionCommand : IRequest<bool>, IRequest
    {
    }

}
