using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using FluentValidation;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RenameExercise;

public sealed class RenameExerciseValidator : AbstractValidator<RenameExerciseCommand>
{
    public RenameExerciseValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.WorkoutId).NotEmpty();
        RuleFor(x => x.ExerciseId).NotEmpty();

        RuleFor(x => x.NewName)
            .NotEmpty()
            .MinimumLength(ExerciseName.MinLength)
            .MaximumLength(ExerciseName.MaxLength);
    }
}