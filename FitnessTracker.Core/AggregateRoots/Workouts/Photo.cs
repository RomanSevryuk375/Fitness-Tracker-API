using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workouts;

public sealed class Photo : Entity
{
    public Guid WorkoutId { get; private set; }
    public FilePath FilePath { get; private set; }

    public Workout? Workout { get; private set; }

    private Photo(
        Guid id,
        Guid workoutId,
        FilePath filePath,
        DateTime createdAt)
    {
        Id = id;
        WorkoutId = workoutId;
        FilePath = filePath;
        CreatedAt = createdAt;
    }

    public static Result<Photo> Create(Guid workoutId, FilePath filePath)
    {
        var photo = new Photo(
            id: Guid.NewGuid(),
            workoutId,
            filePath,
            createdAt: DateTime.UtcNow);

        return Result<Photo>.Success(photo);
    }
}