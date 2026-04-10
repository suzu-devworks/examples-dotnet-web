using Examples.Web.WebApi.Grpc.Inspection;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Examples.Web.WebApi.Grpc.Services.Inspection;

public partial class InspectorService
{

    public override Task<Empty> Validation(ValidationRequest request, ServerCallContext context)
    {
        _logger.LogInformation("The message is received from {Name}", request.Name);
        return Task.FromResult(new Empty());
    }
}
