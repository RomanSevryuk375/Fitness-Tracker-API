using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.AttachPhotoToWorkout;

public sealed record AttachPhotoToWorkoutCommand : ICommand, IUserBoundWorkout
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
    public Stream Content { get; init; } = Stream.Null;
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
}