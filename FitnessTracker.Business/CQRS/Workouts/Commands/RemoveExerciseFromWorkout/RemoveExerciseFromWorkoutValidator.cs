using FluentValidation;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RemoveExerciseFromWorkout;

public sealed class RemoveExerciseFromWorkoutValidator
    : AbstractValidator<RemoveExerciseFromWorkoutCommand>
{
    public RemoveExerciseFromWorkoutValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.WorkoutId).NotEmpty();
        RuleFor(x => x.ExerciseId).NotEmpty();
    }
}