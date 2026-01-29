using FitnessTracker.Core.Abstractions;

namespace FitnessTracker.Core.Entities;

public class ExerciseEntity : IDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string WorkoutId { get; private set; } = Guid.NewGuid().ToString();
    public string Name { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public WorkoutEntity? Workout { get; private set; }
    public List<SetEntity> Sets { get; private set; } = [];

    private ExerciseEntity() { }

    private ExerciseEntity(string id, string workoutId, string name, DateTime createdAt)
    {
        Id = id;
        WorkoutId = workoutId;
        Name = name;
        CreatedAt = createdAt;
    }

    public static (ExerciseEntity? exercise, List<string> errors) Create (string id, string workoutId, string name, DateTime CreateAt)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(id))
            errors.Add("Id can not be empty");
        if (string.IsNullOrWhiteSpace(workoutId))
            errors.Add("WorkoutId can not be empty");
        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name can not be empty");

        if (errors.Any())
            return (null, errors);

        var exercise = new ExerciseEntity(id, workoutId, name, CreateAt);

        return (exercise, []);
    }

    public void AddSet(SetEntity set) => Sets.Add(set);
}
