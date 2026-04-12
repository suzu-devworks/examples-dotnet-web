using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Examples.Web.Infrastructure.GrpcClient;

/// <summary>
/// Interceptor that logs gRPC call details and errors.
/// </summary>
/// <param name="logger">The logger instance to use for logging.</param>
public partial class LoggingInterceptor(ILogger<LoggingInterceptor> logger) : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger = logger;

    /// <inheritdoc/>
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
                TRequest request,
                ClientInterceptorContext<TRequest, TResponse> context,
                AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(request, context);

        return new AsyncUnaryCall<TResponse>(
            HandleResponse(call.ResponseAsync, context),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    private async Task<TResponse> HandleResponse<TRequest, TResponse>(Task<TResponse> inner,
        ClientInterceptorContext<TRequest, TResponse> context)
        where TRequest : class
        where TResponse : class
    {
        LogGrpcStart(context.Method.FullName);
        try
        {
            var response = await inner;
            LogGrpcSuccess(context.Method.FullName);
            return response;
        }
        catch (Exception ex)
        {
            LogGrpcError(context.Method.FullName, ex);
            throw;
        }
    }

    [LoggerMessage(0, LogLevel.Information, "Starting gRPC call: {Name}")]
    private partial void LogGrpcStart(string name);

    [LoggerMessage(1, LogLevel.Information, "gRPC call succeeded: {Name}")]
    private partial void LogGrpcSuccess(string name);

    [LoggerMessage(2, LogLevel.Error, "gRPC call failed: {Name}")]
    private partial void LogGrpcError(string name, Exception ex);

}
