using FitnessTracker.Business.Abstractions;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetExerciseHistory;

public sealed record GetExerciseHistoryQuery 
    : IQuery<Result<IReadOnlyList<ExerciseHistoryItemDto>>>
{
    public Guid UserId { get; init; }
    public string ExerciseName { get; init; } = string.Empty;
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
}
