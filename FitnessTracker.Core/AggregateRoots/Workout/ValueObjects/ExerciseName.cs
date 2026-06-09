using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workout.ValueObjects;

public sealed class ExerciseName
{
    private const int MinLength = 5;
    private const int MaxLength = 128;

    public string Value { get; }

    private ExerciseName(string name)
    {
        Value = name;
    }

    public static Result<ExerciseName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<ExerciseName>.Failure(Error.Validation<ExerciseName>(
                "Name can not be empty"));
        }

        var trimmedName = name.Trim();

        if (trimmedName.Length < MinLength || trimmedName.Length > MaxLength)
        {
            return Result<ExerciseName>.Failure(Error.Validation<ExerciseName>(
                $"Name must be between {MinLength} and {MaxLength} characters."));
        }

        return Result<ExerciseName>.Success(new ExerciseName(name));
    }

    public static implicit operator string(ExerciseName name) => name.Value;

    public override string ToString() => Value.ToString();
}
