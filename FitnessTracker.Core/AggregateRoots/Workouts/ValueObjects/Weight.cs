using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;

public readonly record struct Weight
{
    public const int MinValue = 0;
    public const int MaxValue = 2000;

    public double Value { get; }

    private Weight(double value)
    {
        Value = value;
    }

    public static Result<Weight> Create(double weight)
    {
        if (weight < MinValue)
        {
            return Result<Weight>.Failure(Error.Validation<Weight>(
                "Weight cannot be negative."));
        }

        if (weight > MaxValue)
        {
            return Result<Weight>.Failure(Error.Validation<Weight>(
                "Weight exceeds maximum realistic limits."));
        }

        return Result<Weight>.Success(new Weight(weight));
    }

    public static implicit operator double(Weight weight) => weight.Value;
    public override string ToString() => $"{Value:0.##} кг";
}