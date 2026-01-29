using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.Enums;

namespace FitnessTracker.Core.Entities;

public class WorkoutEntity : IDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public WorkoutType Type { get; private set; }
    public TimeSpan Duration { get; private set; }
    public double CaloriesBurned { get; private set; }
    public DateTime WorkoutDate { get; private set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public UserEntity? User { get; private set; }
    public List<ExerciseEntity> Exercises { get; private set; } = [];
    public List<PhotoEntity> Photos { get; private set; } = [];

    private WorkoutEntity() { }

    private WorkoutEntity(string id, string userId, string title, WorkoutType type, TimeSpan duration, double caloriesBurned, DateTime workoutDate, DateTime createdAt)
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

    public static (WorkoutEntity? workout, List<string> errors) Create(string id, string userId, string title, WorkoutType type, TimeSpan duration, double caloriesBurned, DateTime workoutDate, DateTime createdAt)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(id))
            errors.Add("Id can not be empty");
        if (string.IsNullOrWhiteSpace(userId))
            errors.Add("UserId can not be empty");
        if (string.IsNullOrWhiteSpace(title))
            errors.Add("Title can not be empty");
        if (duration.TotalMinutes <= 0)
            errors.Add("Duration can not be negative");
        if (caloriesBurned < 0)
            errors.Add("Calories can not be negative");
        if (workoutDate > DateTime.UtcNow.AddDays(1))
            errors.Add("Workout date can not be in the future");

        if (errors.Any())
            return (null, errors);

        var workout = new WorkoutEntity(id, userId, title, type, duration, caloriesBurned, workoutDate, createdAt);

        return (workout, []);
    }

    public void Update(string? title, WorkoutType? type, TimeSpan? duration, double? caloriesBurned, DateTime? workoutDate)
    {
        if (title is not null)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty");
            Title = title;
        }

        if (type.HasValue)
            Type = type.Value;

        if (duration.HasValue)
        {
            if (duration.Value.TotalMinutes <= 0) throw new ArgumentException("Duration must be positive");
            Duration = duration.Value;
        }

        if (caloriesBurned.HasValue)
        {
            if (caloriesBurned.Value < 0) throw new ArgumentException("Calories cannot be negative");
            CaloriesBurned = caloriesBurned.Value;
        }

        if (workoutDate.HasValue)
        {
            if (workoutDate.Value > DateTime.UtcNow.AddDays(1)) throw new ArgumentException("Date cannot be in future");
            WorkoutDate = workoutDate.Value;
        }
    }

    public void AddExercise(ExerciseEntity exercise) => Exercises.Add(exercise);

    public void AddPhoto(PhotoEntity photo) => Photos.Add(photo);
}
