using FitnessTracker.Business.Abstractions;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Workouts.Queries.GetWorkoutPhoto;

public sealed record GetWorkoutPhotoQuery : IQuery<Result<FileStreamResponse>>
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
    public string FileName { get; init; } = string.Empty;
}
