using Examples.Web.Infrastructure.GrpcClient;
using Examples.Web.WebApi.Grpc.Inspection;

namespace Examples.Web.WebApi.Grpc.Services.Inspection;

public partial class InspectorService(
    IGreeterService greeterService,
    ILogger<InspectorService> logger
    ) : Inspector.InspectorBase
{
    private readonly IGreeterService _greeterService = greeterService;
    private readonly ILogger<InspectorService> _logger = logger;

}
