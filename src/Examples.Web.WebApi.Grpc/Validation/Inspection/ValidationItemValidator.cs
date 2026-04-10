using Examples.Web.WebApi.Grpc.Inspection;
using FluentValidation;

namespace Examples.Web.WebApi.Grpc.Validation.Inspection;

public class ValidationItemValidator : AbstractValidator<ValidationItem>
{
    public ValidationItemValidator(bool indexed = false)
    {
        var prefix = indexed ? "[{CollectionIndex}]." : string.Empty;

        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage($"{prefix}Id is required.");
        RuleFor(x => x.Quantity)
            .IsWithinRange(1, 100)
            .WithMessage($"{prefix}Quantity must be between 1 and 100.");
    }
}
