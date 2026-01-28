using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.Enums;

namespace FitnessTracker.Core.Entities;

public class WorkoutEntity : IDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public WorkoutType Type { get; set; }
    public TimeSpan Duration { get; set; }
    public double CaloriesBurned { get; set; }
    public DateTime WorkoutDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public UserEntity? User { get; set; }
    public List<ExerciseEntity> Exercises { get; set; } = [];
    public List<PhotoEntity> Photos { get; set; } = [];
}
