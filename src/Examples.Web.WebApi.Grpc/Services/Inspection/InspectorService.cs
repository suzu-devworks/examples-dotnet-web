using Examples.Web.WebApi.Grpc.Inspection;

namespace Examples.Web.WebApi.Grpc.Services.Inspection;

public partial class InspectorService(ILogger<InspectorService> logger) : Inspector.InspectorBase
{
    private readonly ILogger<InspectorService> _logger = logger;

}
