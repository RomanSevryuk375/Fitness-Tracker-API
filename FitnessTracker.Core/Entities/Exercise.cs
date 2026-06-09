using FitnessTracker.Core.Abstractions;
using Shared.Result;

namespace FitnessTracker.Core.Entities;

public class Exercise : IDocument
{
    public Guid Id { get; private set; } 
    public Guid WorkoutId { get; private set; } 
    public string Name { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; set; } 
    public Workout? Workout { get; private set; }
    public List<Set> Sets { get; private set; } = [];

    private Exercise(Guid id, Guid workoutId, string name, DateTime createdAt)
    {
        Id = id;
        WorkoutId = workoutId;
        Name = name;
        CreatedAt = createdAt;
    }

    public static Result<Exercise> Create (Guid workoutId, string name)
    {
        var errors = new List<string>();

        if (workoutId == Guid.Empty)
        {
            errors.Add("workoutId must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("name can not be empty");
        }

        if (errors.Count != 0)
        {
            return Result<Exercise>.Failure(Error.Validation(
                $"{nameof(Exercise)}.Invalid",
                string.Join("; ", errors)));
        }

        var exercise = new Exercise(
            Guid.NewGuid(), 
            workoutId, 
            name.Trim(), 
            DateTime.UtcNow);

        return Result<Exercise>.Success(exercise);
    }

    public void AddSet(Set set) => Sets.Add(set);
}
