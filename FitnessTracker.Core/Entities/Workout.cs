using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.Enums;
using Shared.Result;

namespace FitnessTracker.Core.Entities;

public class Workout : IDocument
{
    public Guid Id { get; set; } 
    public Guid UserId { get; private set; } 
    public string Title { get; private set; } 
    public WorkoutType Type { get; private set; }
    public TimeSpan Duration { get; private set; }
    public int CaloriesBurned { get; private set; }
    public DateTime WorkoutDate { get; private set; }
    public DateTime CreatedAt { get; set; }
    public UserEntity? User { get; private set; }
    public List<Exercise> Exercises { get; private set; } = [];
    public List<Photo> Photos { get; private set; } = [];

    private Workout(
        Guid id, 
        Guid userId, 
        string title, 
        WorkoutType type, 
        TimeSpan duration, 
        int caloriesBurned, 
        DateTime workoutDate, 
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
        string title, 
        WorkoutType type, 
        TimeSpan duration, 
        int caloriesBurned, 
        DateTime workoutDate)
    {
        var errors = new List<string>();

        if (userId == Guid.Empty)
        {
            errors.Add("userId must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            errors.Add("Title can not be empty");
        }

        if (duration.TotalMinutes <= 0)
        {
            errors.Add("Duration can not be negative");
        }

        if (caloriesBurned < 0)
        {
            errors.Add("Calories can not be negative");
        }

        if (workoutDate > DateTime.UtcNow.AddDays(1))
        {
            errors.Add("Workout date can not be in the future");
        }

        if (errors.Count != 0)
        {
            return Result<Workout>.Failure(Error.Validation(
                $"{nameof(Workout)}.Invalid",
                string.Join("; ", errors)));
        }

        var workout = new Workout(
            Guid.NewGuid(), 
            userId, 
            title.Trim(), 
            type, 
            duration, 
            caloriesBurned, 
            workoutDate, 
            DateTime.UtcNow);

        return Result<Workout>.Success(workout);
    }

    public Result Update(
        string? title, 
        WorkoutType? type, 
        TimeSpan? duration, 
        int? caloriesBurned, 
        DateTime? workoutDate)
    {
        var errors = new List<string>();

        if (title is not null && string.IsNullOrWhiteSpace(title))
        {
            errors.Add("Title cannot be empty.");
        }

        if (duration.HasValue && duration.Value.TotalMinutes <= 0)
        {
            errors.Add("Duration must be positive.");
        }

        if (caloriesBurned.HasValue && caloriesBurned.Value < 0)
        {
            errors.Add("Calories cannot be negative.");
        }

        if (workoutDate.HasValue && workoutDate.Value > DateTime.UtcNow.AddDays(1))
        {
            errors.Add("Date cannot be in the future.");
        }

        if (errors.Count != 0)
        {
            return Result.Failure(Error.Validation(
                $"{nameof(Workout)}.Invalid",
                string.Join("; ", errors)));
        }

        if (title is not null)
        {
            Title = title;
        }

        if (type.HasValue)
        {
            Type = type.Value;
        }

        if (duration.HasValue)
        {
            Duration = duration.Value;
        }

        if (caloriesBurned.HasValue)
        {
            CaloriesBurned = caloriesBurned.Value;
        }

        if (workoutDate.HasValue)
        {
            WorkoutDate = workoutDate.Value;
        }

        return Result.Success();
    }

    public void AddExercise(Exercise exercise) => Exercises.Add(exercise);

    public void AddPhoto(Photo photo) => Photos.Add(photo);
}
