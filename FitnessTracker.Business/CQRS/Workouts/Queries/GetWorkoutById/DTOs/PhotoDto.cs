namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutById.DTOs;

public sealed record PhotoDto
{
    public string FilePath { get; init; } = string.Empty;
}
