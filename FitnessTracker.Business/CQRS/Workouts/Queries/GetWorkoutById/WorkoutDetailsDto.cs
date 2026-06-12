using FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutById.DTOs;

namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutById;

public sealed record WorkoutDetailsDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public TimeSpan Duration { get; init; }
    public int CaloriesBurned { get; init; }
    public DateTime WorkoutDate { get; init; }

    public List<ExerciseDto> Exercises { get; set; } = [];
    public List<PhotoDto> Photos { get; set; } = [];
}
