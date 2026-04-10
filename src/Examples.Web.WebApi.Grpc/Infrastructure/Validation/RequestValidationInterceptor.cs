using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Examples.Web.Infrastructure.Validation;

/// <summary>
/// Validates incoming gRPC requests using FluentValidation.
/// Throws RpcException with InvalidArgument status if validation fails.
/// </summary>
public class RequestValidationInterceptor : Interceptor
{
    /// <inheritdoc />
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var httpContext = context.GetHttpContext();
        var validator = httpContext.RequestServices.GetService<IValidator<TRequest>>();

        if (validator is not null)
        {
            var result = await validator.ValidateAsync(request, context.CancellationToken);

            if (!result.IsValid)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.ErrorMessage));
                throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
            }
        }

        return await continuation(request, context);
    }
}
