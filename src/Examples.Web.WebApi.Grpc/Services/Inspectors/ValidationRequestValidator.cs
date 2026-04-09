using Examples.Web.WebApi.Grpc.Inspectors;
using FluentValidation;

namespace Examples.Web.WebApi.Grpc.Services.Inspectors;

public class ValidationRequestValidator : AbstractValidator<ValidationRequest>
{
    public ValidationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters.");
    }
}
