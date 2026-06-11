using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RemoveSetFromExercise;

public sealed record RemoveSetFromExerciseCommand : ICommand
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
    public Guid ExerciseId { get; init; }
    public Guid SetId { get; init; }
}