using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;

public readonly record struct WorkoutDate
{
    public DateTime Value { get; }

    private WorkoutDate(DateTime value)
    {
        Value = value;
    }

    public static Result<WorkoutDate> Create(DateTime date, DateTime utcNow)
    {
        var maxAllowedDate = utcNow.AddDays(1);

        if (date > maxAllowedDate)
        {
            return Result<WorkoutDate>.Failure(Error.Validation<WorkoutDate>(
                "Workout date cannot be in the future."));
        }

        var minAllowedDate = utcNow.AddYears(-10);
        if (date < minAllowedDate)
        {
            return Result<WorkoutDate>.Failure(Error.Validation<WorkoutDate>(
                "Workout date is too far in the past."));
        }

        return Result<WorkoutDate>.Success(new WorkoutDate(date.ToUniversalTime()));
    }
    public static WorkoutDate Restore(DateTime value) => new(value);

    public static implicit operator DateTime(WorkoutDate date) => date.Value;
    public override string ToString() => Value.ToString("yyyy-MM-dd HH:mm");
}