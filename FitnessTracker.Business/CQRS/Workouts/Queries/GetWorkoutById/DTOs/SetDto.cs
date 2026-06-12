namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutById.DTOs;

public sealed record SetDto
{
    public int Reps { get; init; }
    public string Weight { get; init; } = string.Empty;
}
