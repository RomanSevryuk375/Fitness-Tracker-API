namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutById.DTOs;

public sealed record ExerciseDto
{
    public Guid Id { get; init; }
    public string EserciseName { get; init; } = string.Empty;

    public List<SetDto> Sets { get; init; } = [];
}
