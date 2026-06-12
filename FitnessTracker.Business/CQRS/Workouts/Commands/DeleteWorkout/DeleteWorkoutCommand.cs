using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.DeleteWorkout;

public sealed record DeleteWorkoutCommand : ICommand, IUserBoundWorkout
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
}
