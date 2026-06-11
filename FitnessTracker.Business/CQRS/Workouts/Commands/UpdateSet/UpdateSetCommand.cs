using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Workouts.Commands.UpdateSet;

public sealed record UpdateSetCommand : ICommand
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
    public Guid ExerciseId { get; init; }
    public Guid SetId { get; init; }
    public int? Reps { get; init; }
    public double? Weight { get; init; }
}