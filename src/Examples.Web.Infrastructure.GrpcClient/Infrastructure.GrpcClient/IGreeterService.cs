namespace Examples.Web.Infrastructure.GrpcClient;

public interface IGreeterService
{
    Task<string> GetMessageAsync(string name, CancellationToken cancellationToken = default);
}
