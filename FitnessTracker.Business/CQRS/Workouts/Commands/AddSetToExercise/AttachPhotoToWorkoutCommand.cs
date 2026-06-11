using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.AddSetToExercise;

public sealed record AddSetToExerciseCommand : ICommand
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
    public Guid ExerciseId { get; init; }
    public int Reps { get; init; }
    public double Weight { get; init; }
}
