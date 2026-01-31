using FitnessTracker.Business.DTOs;
using FluentValidation;

namespace FitnessTracker.Business.Validators;

public class CreateWorkoutRequestValidator : AbstractValidator<CreateWorkoutRequest>
{
    public CreateWorkoutRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title can not be empty")
            .MaximumLength(100).WithMessage("Title can not be longer than 100 symbols");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Workout should last more than 0 minutes");

        RuleFor(x => x.WorkoutDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Workout can not be in future");

        RuleFor(x => x.CaloriesBurned)
            .NotEmpty().WithMessage("CaloriesBurned can not be empty")
            .LessThan(4000).WithMessage("CaloriesBurned can not be more than 4000");

        RuleForEach(x => x.Exercises).SetValidator(new CreateExerciseRequestValidator());
    }
}
