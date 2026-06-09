using FitnessTracker.Core.Abstractions;
using Shared.Result;

namespace FitnessTracker.Core.Entities;

public class Photo : IDocument
{
    public Guid Id { get; set; } 
    public Guid WorkoutId { get; private set; } 
    public string FilePath { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; set; } 
    public Workout? Workout { get; private set; }

    private Photo(
        Guid id, 
        Guid workoutId,  
        string filePath,  
        DateTime createdAt)
    {
        Id = id;
        WorkoutId = workoutId;
        FilePath = filePath;
        CreatedAt = createdAt;
    }

    public static Result<Photo> Create ( 
        Guid workoutId, 
        string filePath)
    {
        var errors = new List<string>();

        if (workoutId == Guid.Empty)
        {
            errors.Add("workoutId must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(filePath))
        {
            errors.Add("FilePath can not be empty");
        }

        if (errors.Count != 0)
        {
            return Result<Photo>.Failure(Error.Validation(
                $"{nameof(Photo)}.Invalid",
                string.Join("; ", errors)));
        }

        var photo = new Photo(
            Guid.NewGuid(), 
            workoutId, 
            filePath.Trim(), 
            DateTime.UtcNow);

        return Result<Photo>.Success(photo);
    }
}
