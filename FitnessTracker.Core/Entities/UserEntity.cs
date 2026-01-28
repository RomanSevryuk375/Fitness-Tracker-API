using FitnessTracker.Core.Abstractions;

namespace FitnessTracker.Core.Entities;

public class UserEntity : IDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<WorkoutEntity> Workouts { get; set; } = [];
}
