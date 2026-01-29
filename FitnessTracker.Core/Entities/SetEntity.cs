using FitnessTracker.Core.Abstractions;

namespace FitnessTracker.Core.Entities;

public class SetEntity : IDocument
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ExerciseId { get; private set; } = Guid.NewGuid().ToString();
    public int Reps { get; private set; }
    public double Weight { get; private set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ExerciseEntity? Exercise { get; private set; }

    private SetEntity() { }

    private SetEntity(string id, string exerciseId, int reps, double weight, DateTime createdAt)
    {
        Id = id;
        ExerciseId = exerciseId;
        Reps = reps;
        Weight = weight;
        CreatedAt = createdAt;
    }

    public static (SetEntity? set, List<string> errors) Create(string id, string exerciseId, int reps, double weight, DateTime createdAt)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(id))
            errors.Add("Id can not be empty");
        if (string.IsNullOrWhiteSpace(exerciseId))
            errors.Add("ExerciseId can not be empty");
        if (reps <= 0)
            errors.Add("Reps can not be negative or zero");
        if (weight <= 0)
            errors.Add("Weight can not be negative or zero");

        if (errors.Any())
            return (null, errors);

        var set = new SetEntity(id, exerciseId, reps, weight, createdAt);

        return (set, []);
    }
}
