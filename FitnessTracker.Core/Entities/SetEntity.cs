using FitnessTracker.Core.Abstractions;

namespace FitnessTracker.Core.Entities;

public class SetEntity : IDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ExerciseId { get; set; } = Guid.NewGuid().ToString();
    public int Reps { get; set; }
    public double Weight { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ExerciseEntity? Exercise { get; set; }
}
