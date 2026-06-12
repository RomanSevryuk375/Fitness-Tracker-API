using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Enums;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.UpdateWorkout;

public sealed record UpdateWorkoutCommand : ICommand<Guid>, IUserBoundWorkout
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
    public string? Title { get; init; } = string.Empty;
    public WorkoutType? Type { get; init; }
    public TimeSpan? Duration { get; init; }
    public int? CaloriesBurned { get; init; }
    public DateTime? WorkoutDate { get; init; }
}