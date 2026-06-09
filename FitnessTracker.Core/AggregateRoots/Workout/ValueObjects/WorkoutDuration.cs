using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workout.ValueObjects;

public readonly record struct WorkoutDuration
{
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(12);

    public TimeSpan Value { get; }

    private WorkoutDuration(TimeSpan value)
    {
        Value = value;
    }

    public static Result<WorkoutDuration> Create(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
        {
            return Result<WorkoutDuration>.Failure(Error.Validation<WorkoutDuration>(
                "Workout duration must be greater than zero."));
        }

        if (duration > MaxDuration)
        {
            return Result<WorkoutDuration>.Failure(Error.Validation<WorkoutDuration>(
                $"Workout duration cannot exceed {MaxDuration.TotalHours} hours."));
        }

        return Result<WorkoutDuration>.Success(new WorkoutDuration(duration));
    }

    public static implicit operator TimeSpan(WorkoutDuration duration) => duration.Value;
    public override string ToString() => Value.ToString("g");
}