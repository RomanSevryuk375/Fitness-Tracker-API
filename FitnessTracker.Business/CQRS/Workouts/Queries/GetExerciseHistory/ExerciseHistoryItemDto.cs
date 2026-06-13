namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetExerciseHistory;

public sealed record ExerciseHistoryItemDto
{
    public DateTime Date { get; init; }
    public double MaxWeight { get; init; }
    public double TotalVolume { get; init; }
    public int TotalReps { get; init; }
}
