using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Examples.Web.Infrastructure.GrpcClient;

/// <summary>
/// Interceptor that handles unhandled exceptions in gRPC calls.
/// </summary>
public class UnhandledExceptionInterceptor : Interceptor
{
    /// <inheritdoc/>
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(request, context);

        return new AsyncUnaryCall<TResponse>(
            HandleResponse(call.ResponseAsync),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    private static async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> inner)
    {
        try
        {
            return await inner;
        }
        catch (RpcException ex)
        {
            throw new GrpcClientException($"gRPC call failed with status code {ex.StatusCode}", ex);
        }
        catch (Exception ex)
        {
            throw new GrpcClientException("gRPC call failed", ex);
        }
    }
}
