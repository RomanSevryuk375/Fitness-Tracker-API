namespace FitnessTracker.Business.DTOs;

public record CreateWorkoutRequest
{
    public string Title { get; init; } = null!;
    public int TypeId { get; init; }
    public int DurationMinutes { get; init; }
    public int CaloriesBurned { get; init; }
    public DateTime WorkoutDate { get; init; }
    public List<CreateExerciseRequest> Exercises { get; init; } = [];
};
