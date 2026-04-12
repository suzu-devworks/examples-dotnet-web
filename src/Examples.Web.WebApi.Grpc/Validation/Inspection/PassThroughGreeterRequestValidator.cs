using Examples.Web.WebApi.Grpc.Inspection;
using FluentValidation;

namespace Examples.Web.WebApi.Grpc.Validation.Inspection;

public class PassThroughGreeterRequestValidator : AbstractValidator<PassThroughGreeterRequest>
{
    public PassThroughGreeterRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }
}
