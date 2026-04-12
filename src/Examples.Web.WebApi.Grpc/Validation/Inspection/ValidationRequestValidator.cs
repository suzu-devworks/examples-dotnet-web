using Examples.Web.WebApi.Grpc.Inspection;
using FluentValidation;

namespace Examples.Web.WebApi.Grpc.Validation.Inspection;

public class ValidationRequestValidator : AbstractValidator<ValidationRequest>
{
    public ValidationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters.");
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status must be a valid enum value.");
        RuleFor(x => x.CreatedAt)
            .NotEmpty()
            .WithMessage("CreatedAt is required.");

        RuleFor(x => x.DateYmdFrom)
            .NotEmpty()
            .WithMessage("DateYmdFrom is required.")
            .IsValidDate()
            .WithMessage("DateYmdFrom must be a valid date in the format yyyy-MM-dd.");
        RuleFor(x => x.DateYmdTo)
            .IsValidDate()
            .WithMessage("DateYmdTo must be a valid date in the format yyyy-MM-dd.")
            .DependentRules(() =>
            {
                RuleFor(x => x.DateYmdTo)
                    .GreaterThanOrEqualTo(x => x.DateYmdFrom)
                    .WithMessage("DateYmdTo must be greater than or equal to DateYmdFrom.");
            })
            .When(x => !string.IsNullOrEmpty(x.DateYmdTo));

        RuleFor(x => x.TimeHmsFrom)
            .NotEmpty()
            .WithMessage("TimeHmsFrom is required.")
            .IsValidTime()
            .WithMessage("TimeHmsFrom must be a valid time in the format HH:mm:ss.");
        RuleFor(x => x.TimeHmsTo)
            .IsValidTime()
            .WithMessage("TimeHmsTo must be a valid time in the format HH:mm:ss.")
            .When(x => !string.IsNullOrEmpty(x.TimeHmsTo))
            .GreaterThan(x => x.TimeHmsFrom)
            .WithMessage("TimeHmsTo must be greater than TimeHmsFrom.")
            .When(x => !string.IsNullOrEmpty(x.TimeHmsFrom) && !string.IsNullOrEmpty(x.TimeHmsTo));

        RuleForEach(x => x.Items)
            .SetValidator(new ValidationItemValidator(indexed: true));
    }
}
