using FitnessTracker.Core.Abstractions;

namespace FitnessTracker.Core.Entities;

public class PhotoEntity : IDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string WorkoutId { get; private set; } = Guid.NewGuid().ToString();
    public string FilePath { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public WorkoutEntity? Workout { get; private set; }

    private PhotoEntity() { }

    private PhotoEntity(string id, string workoutId,  string filePath,  DateTime createdAt)
    {
        Id = id;
        WorkoutId = workoutId;
        FilePath = filePath;
        CreatedAt = createdAt;
    }

    public static (PhotoEntity? photo, List<string> errors) Create (string id, string workoutId, string filePath, DateTime createdAt)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(id))
            errors.Add("Id can not be empty");
        if (string.IsNullOrWhiteSpace(workoutId))
            errors.Add("WorkoutId can not be empty");
        if (string.IsNullOrWhiteSpace(filePath))
            errors.Add("FilePath can not be empty");

        if (errors.Any())
            return (null, errors);

        var photo = new PhotoEntity(id, workoutId, filePath, createdAt);

        return (photo, []);
    }
}
