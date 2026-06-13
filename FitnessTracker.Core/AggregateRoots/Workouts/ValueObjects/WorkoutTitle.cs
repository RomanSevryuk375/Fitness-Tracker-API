using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;

public readonly record struct WorkoutTitle
{
    public const int MaxLength = 128;

    public string Value { get; }

    public WorkoutTitle(string title)
    {
        Value = title;
    }

    public static Result<WorkoutTitle> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result<WorkoutTitle>.Failure(Error.Validation<WorkoutTitle>(
                "Login can not be empty"));
        }

        if (title.Length > MaxLength)
        {
            return Result<WorkoutTitle>.Failure(Error.Validation<WorkoutTitle>(
                $"Workout title should be shorter then {MaxLength} symbols"));
        }

        return Result<WorkoutTitle>.Success(new WorkoutTitle(title));
    }

    public static implicit operator string(WorkoutTitle title) => title.Value;

    public override string ToString() => Value.ToString();
}
