using Examples.Web.WebApi.Grpc.Inspection;
using Grpc.Core;

namespace Examples.Web.WebApi.Grpc.Services.Inspection;

public partial class InspectorService
{
    public override async Task<PassThroughGreeterReply> PassThroughGreeter(PassThroughGreeterRequest request, ServerCallContext context)
    {
        var message = await _greeterService.GetMessageAsync(request.Name, context.CancellationToken);

        var reply = new PassThroughGreeterReply
        {
            Message = $"The message is received from {request.Name}: {message}"
        };

        return reply;
    }
}
