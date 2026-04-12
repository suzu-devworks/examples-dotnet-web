namespace Examples.Web.Infrastructure.GrpcClient;

public class GrpcClientException(string message, Exception? e) : Exception(message, e)
{
    public GrpcClientException(string message) : this(message, null)
    {
    }
}
