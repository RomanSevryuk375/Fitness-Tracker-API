using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Enums;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutList;

public sealed record GetWorkoutListQuery 
    : IQuery<Result<IReadOnlyList<WorkoutListItemDto>>>
{
    public Guid UserId { get; init; }
    public WorkoutType? Type { get; init; }
    public int Skip { get; init; }
    public int Take { get; init; }
}
