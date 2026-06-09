using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workout.ValueObjects;

public readonly record struct Repetitions
{
    private const int MaxValue = 1000;
    private const int MinValue = 0;

    public int Value { get; }

    private Repetitions(int value)
    {
        Value = value;
    }

    public static Result<Repetitions> Create(int reps)
    {
        if (reps <= MinValue)
        {
            return Result<Repetitions>.Failure(Error.Validation<Repetitions>(
                "Repetitions must be greater than zero."));
        }

        if (reps > MaxValue)
        {
            return Result<Repetitions>.Failure(Error.Validation<Repetitions>(
                "Repetitions cannot exceed 1000."));
        }

        return Result<Repetitions>.Success(new Repetitions(reps));
    }

    public static implicit operator int(Repetitions reps) => reps.Value;
    public override string ToString() => Value.ToString();
}