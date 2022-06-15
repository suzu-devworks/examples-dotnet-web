using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

namespace Examples.WebApi.Applications.CQRS.Commands
{
    public class DoExceptionCommandHandler : IRequestHandler<DoExceptionCommand, bool>
    {
        public Task<bool> Handle(DoExceptionCommand request, CancellationToken cancellationToken)
        {
            throw new ApplicationException("<<Throw!!>>");
        }
    }

}
