using FitnessTracker.Core.Abstractions;
using Shared.Result;

namespace FitnessTracker.Core.Entities;

public class Set : IDocument
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; private set; }
    public int Reps { get; private set; }
    public double Weight { get; private set; }
    public DateTime CreatedAt { get; set; }
    public Exercise? Exercise { get; private set; }

    private Set(
        Guid id, 
        Guid exerciseId, 
        int reps, 
        double weight, 
        DateTime createdAt)
    {
        Id = id;
        ExerciseId = exerciseId;
        Reps = reps;
        Weight = weight;
        CreatedAt = createdAt;
    }

    public static Result<Set> Create(Guid exerciseId, int reps, double weight)
    {
        var errors = new List<string>();

        if (exerciseId == Guid.Empty)
        {
            errors.Add("exerciseId must not be empty.");
        }

        if (reps <= 0)
        {
            errors.Add("Reps can not be negative or zero");
        }

        if (weight <= 0)
        {
            errors.Add("Weight can not be negative or zero");
        }

        if (errors.Any())
        {
            return Result<Set>.Failure(Error.Validation(
                $"{nameof(Set)}.Invalid",
                string.Join("; ", errors)));
        }

        var set = new Set(
            Guid.NewGuid(), 
            exerciseId, 
            reps, 
            weight, 
            DateTime.UtcNow);

        return Result<Set>.Success(set);
    }
}
