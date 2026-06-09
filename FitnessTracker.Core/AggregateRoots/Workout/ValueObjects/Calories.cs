using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workout.ValueObjects;

public readonly record struct Calories
{
    public const int MinCalories = 0;
    public const int MaxCalories = 4000;

    public int Value { get; }

    private Calories(int value)
    {
        Value = value;
    }

    public static Result<Calories> Create(int calories)
    {
        if (calories < MinCalories)
        {
            return Result<Calories>.Failure(Error.Validation<Calories>(
                "Calories cannot be negative."));
        }

        if (calories > MaxCalories)
        {
            return Result<Calories>.Failure(Error.Validation<Calories>(
                $"Calories cannot exceed {MaxCalories}."));
        }

        return Result<Calories>.Success(new Calories(calories));
    }

    public static implicit operator int(Calories calories) => calories.Value;
    public override string ToString() => Value.ToString();
}