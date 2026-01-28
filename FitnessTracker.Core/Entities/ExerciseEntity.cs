using FitnessTracker.Core.Abstractions;

namespace FitnessTracker.Core.Entities;

public class ExerciseEntity : IDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string WorkoutId { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public WorkoutEntity? Workout { get; set; }
    public List<SetEntity> Sets { get; set; } = [];
}
