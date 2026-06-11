using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using FluentValidation;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.CreateWorkout;

public sealed class CreateWorkoutValidator : AbstractValidator<CreateWorkoutCommand>
{
    public CreateWorkoutValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(WorkoutTitle.MaxLength);

        RuleFor(x => x.Type)
            .IsInEnum()
            .NotEmpty();
    }
}