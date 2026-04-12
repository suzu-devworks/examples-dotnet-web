using Examples.Web.WebApi.Grpc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Examples.Web.Infrastructure.GrpcClient.Internals;

internal class GreeterService(
    Greeter.GreeterClient client,
    IOptionsSnapshot<GrpcClientOptions> options,
    TimeProvider timeProvider,
    ILogger<GreeterService> logger
    ) : IGreeterService
{
    private readonly Greeter.GreeterClient _client = client;
    private readonly GrpcClientOptions _options = options.Value;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly ILogger<GreeterService> _logger = logger;

    public async Task<string> GetMessageAsync(string name, CancellationToken cancellationToken = default)
    {
        var response = await _client.SayHelloAsync(
                new HelloRequest { Name = name },
                deadline: _timeProvider.GetDeadline(_options.Greeter.Timeout),
                cancellationToken: cancellationToken);
        _logger.LogInformation("Greeting: {Message}", response.Message);
        return response.Message;
    }
}
