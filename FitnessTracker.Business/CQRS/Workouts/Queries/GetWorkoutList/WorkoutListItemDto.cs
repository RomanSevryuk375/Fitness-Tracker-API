
namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutList;

public sealed record WorkoutListItemDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public TimeSpan Duration { get; init; }
    public int CaloriesBurned { get; init; }
    public DateTime WorkoutDate { get; init; }
    public int ExercisesCount { get; init; }
}