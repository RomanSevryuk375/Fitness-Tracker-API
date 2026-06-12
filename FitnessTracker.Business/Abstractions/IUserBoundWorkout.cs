namespace FitnessTracker.Business.Abstractions;

public interface IUserBoundWorkout
{
    Guid UserId { get; init; }
    Guid WorkoutId { get; init; }
}
