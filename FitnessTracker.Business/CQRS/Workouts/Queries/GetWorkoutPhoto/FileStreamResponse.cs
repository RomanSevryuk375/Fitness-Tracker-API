namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutPhoto;

public sealed record FileStreamResponse
{
    public Stream Stream { get; init; } = Stream.Null;
    public string ContentType { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
}
