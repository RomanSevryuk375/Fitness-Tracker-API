namespace FitnessTracker.Business.DTOs;

public record WorkoutDto
{
    public string Id { get; init; } = null!;
    public string UserId { get; set; } = null!;
    public string Title { get; init; } = null!;
    public string Type { get; init; } = null!;
    public List<string> ProgressPhotos { get; init; } = [];
    public TimeSpan Duration { get; init; }
    public int CaloriesBurned { get; init; }
    public DateTime WorkoutDate { get; init; }
    public List<ExerciseDto> Exercises { get; init; } = [];
}