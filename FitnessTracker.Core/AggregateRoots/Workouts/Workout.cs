using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.MediaAttachments;
using FitnessTracker.Core.AggregateRoots.Users;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using FitnessTracker.Core.Enums;
using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workouts;

public sealed class Workout : Entity
{
    private const int ExerciseLimit = 20;
    private const int PhotoLimit = 5;

    private readonly List<Exercise> _exercises = [];
    private readonly List<Photo> _photos = [];

    public Guid UserId { get; private set; }
    public WorkoutTitle Title { get; private set; }
    public WorkoutType Type { get; private set; }
    public WorkoutDuration Duration { get; private set; }
    public Calories CaloriesBurned { get; private set; }
    public WorkoutDate WorkoutDate { get; private set; }

    public User? User { get; private set; }

    public IReadOnlyCollection<Exercise> Exercises => _exercises.AsReadOnly();
    public IReadOnlyCollection<Photo> Photos => _photos.AsReadOnly();

    private Workout(
        Guid id,
        Guid userId,
        WorkoutTitle title,
        WorkoutType type,
        WorkoutDuration duration,
        Calories caloriesBurned,
        WorkoutDate workoutDate,
        DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        Title = title;
        Type = type;
        Duration = duration;
        CaloriesBurned = caloriesBurned;
        WorkoutDate = workoutDate;
        CreatedAt = createdAt;
    }

    public static Result<Workout> Create(
        Guid userId,
        WorkoutTitle title,
        WorkoutType type,
        WorkoutDuration duration,
        Calories calories,
        WorkoutDate workoutDate)
    {
        var workout = new Workout(
            id: Guid.NewGuid(),
            userId,
            title,
            type,
            duration,
            calories,
            workoutDate,
            createdAt: DateTime.UtcNow);

        return Result<Workout>.Success(workout);
    }

    public Result Update(
        WorkoutTitle? title,
        WorkoutType? type,
        WorkoutDuration? duration,
        Calories? calories,
        WorkoutDate? date)
    {
        if (title.HasValue)
        {
            Title = title.Value;
        }

        if (type.HasValue)
        {
            Type = type.Value;
        }

        if (duration.HasValue)
        {
            Duration = duration.Value;
        }

        if (calories.HasValue)
        {
            CaloriesBurned = calories.Value;
        }

        if (date.HasValue)
        {
            WorkoutDate = date.Value;
        }

        return Result.Success();
    }

    public Result AddExercise(Exercise exercise)
    {
        if (_exercises.Count >= ExerciseLimit)
        {
            return Result.Failure(Error.Validation<Workout>(
                "Workout cannot have more than 20 exercises."));
        }

        _exercises.Add(exercise);
        return Result.Success();
    }

    public Result AddPhoto(Photo photo)
    {
        if (_photos.Count >= PhotoLimit)
        {
            return Result.Failure(Error.Validation<Workout>(
                "Cannot attach more than 5 photos to a workout."));
        }

        _photos.Add(photo);
        return Result.Success();
    }
}