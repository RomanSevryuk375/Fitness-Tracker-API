using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.RemoveExerciseFromWorkout;

public sealed record RemoveExerciseFromWorkoutCommand : ICommand, IUserBoundWorkout
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
    public Guid ExerciseId { get; init; }
}