using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.AddExerciseToWorkout;

public sealed record AddExerciseToWorkoutCommand
    : ICommand<Guid>, IUserBoundWorkout
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
    public string Name { get; init; } = string.Empty;
}
