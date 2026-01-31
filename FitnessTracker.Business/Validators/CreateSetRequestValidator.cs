using FitnessTracker.Business.DTOs;
using FluentValidation;

namespace FitnessTracker.Business.Validators;

public class CreateSetRequestValidator : AbstractValidator<CreateSetRequest>
{
    public CreateSetRequestValidator()
    {
        RuleFor(x => x.Reps)
            .NotEmpty().WithMessage("Reps can not be empty")
            .GreaterThan(0).WithMessage("Reps must be greater than 0");

        RuleFor(x => x.Weight)
            .NotEmpty().WithMessage("Weight can not be empty")
            .GreaterThanOrEqualTo(0).WithMessage("Weight must be greater than 0");
    }
}
