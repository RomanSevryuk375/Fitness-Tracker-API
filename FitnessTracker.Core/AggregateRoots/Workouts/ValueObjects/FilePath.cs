using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;

public readonly record struct FilePath
{
    public const int MaxLength = 500;

    public string Value { get; }

    private FilePath(string value)
    {
        Value = value;
    }

    public static Result<FilePath> Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return Result<FilePath>.Failure(Error.Validation<FilePath>(
                "File path cannot be empty or whitespace."));
        }

        var trimmedPath = path.Trim();

        if (trimmedPath.Length > MaxLength)
        {
            return Result<FilePath>.Failure(Error.Validation<FilePath>(
                $"File path cannot exceed {MaxLength} characters."));
        }

        return Result<FilePath>.Success(new FilePath(trimmedPath));
    }

    public static implicit operator string(FilePath path) => path.Value;
    public override string ToString() => Value;
}