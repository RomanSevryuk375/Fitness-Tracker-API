using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Enums;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.CreateWorkout;

public sealed record CreateWorkoutCommand : ICommand<Guid>
{
    public Guid UserId { get; init; }
    public string Title { get; init; } = string.Empty;
    public WorkoutType Type { get; init; }
    public TimeSpan Duration { get; init; }
    public int CaloriesBurned { get; init; }
    public DateTime WorkoutDate { get; init; }
}   
