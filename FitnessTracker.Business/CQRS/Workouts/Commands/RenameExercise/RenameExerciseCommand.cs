using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RenameExercise;

public sealed record RenameExerciseCommand : ICommand, IUserBoundWorkout
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
    public Guid ExerciseId { get; init; }
    public string NewName { get; init; } = string.Empty;
}