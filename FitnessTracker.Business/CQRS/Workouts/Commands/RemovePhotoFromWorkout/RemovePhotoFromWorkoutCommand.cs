using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RemovePhotoFromWorkout;

public sealed record RemovePhotoFromWorkoutCommand 
    : ICommand, IUserBoundWorkout
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
    public Guid PhotoId { get; init; }
}