using FitnessTracker.Business.Abstractions;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutById;

public sealed record GetWorkoutByIdQuery 
    : IQuery<Result<WorkoutDetailsDto>>
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
}
