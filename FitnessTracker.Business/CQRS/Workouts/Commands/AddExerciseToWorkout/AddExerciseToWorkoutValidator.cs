using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using FluentValidation;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.AddExerciseToWorkout;

public class AddExerciseToWorkoutValidator : AbstractValidator<AddExerciseToWorkoutCommand>
{
    public AddExerciseToWorkoutValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.WorkoutId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(ExerciseName.MinLength)
            .MaximumLength(ExerciseName.MaxLength);
    }
}
