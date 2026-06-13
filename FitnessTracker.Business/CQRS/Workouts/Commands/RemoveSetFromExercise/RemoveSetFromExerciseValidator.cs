using FluentValidation;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RemoveSetFromExercise;

public sealed class RemoveSetFromExerciseValidator
    : AbstractValidator<RemoveSetFromExerciseCommand>
{
    public RemoveSetFromExerciseValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.WorkoutId).NotEmpty();
        RuleFor(x => x.ExerciseId).NotEmpty();
        RuleFor(x => x.SetId).NotEmpty();
    }
}