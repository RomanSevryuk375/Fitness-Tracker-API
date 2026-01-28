using FitnessTracker.Core.Abstractions;

namespace FitnessTracker.Core.Entities;

public class PhotoEntity : IDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string WorkoutId { get; set; } = Guid.NewGuid().ToString();
    public string FilePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public WorkoutEntity? Workout { get; set; }
}
