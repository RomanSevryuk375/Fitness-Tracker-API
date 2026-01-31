using FitnessTracker.Business.DTOs;
using FluentValidation;

namespace FitnessTracker.Business.Validators;

public class CreateExerciseRequestValidator : AbstractValidator<CreateExerciseRequest>
{
    public CreateExerciseRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name can not be empty")
            .MaximumLength(100).WithMessage("Name can not be longer than 100 symbols");

        RuleForEach(x => x.Sets)
            .SetValidator(new CreateSetRequestValidator());
    }
}
