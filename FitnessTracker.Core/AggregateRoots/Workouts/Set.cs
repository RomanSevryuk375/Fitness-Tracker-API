using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workouts;

public sealed class Set : Entity
{
    public Guid ExerciseId { get; private set; }
    public Repetitions Reps { get; private set; }
    public Weight Weight { get; private set; }

    public Exercise? Exercise { get; private set; }


    private Set(
        Guid id,
        Guid exerciseId,
        Repetitions repetitions,
        Weight weight,
        DateTime createdAt)
    {
        Id = id;
        ExerciseId = exerciseId;
        Reps = repetitions;
        Weight = weight;
        CreatedAt = createdAt;
    }

    public static Result<Set> Create(Guid exerciseId, Repetitions reps, Weight weight)
    {
        var set = new Set(
            id: Guid.NewGuid(),
            exerciseId,
            repetitions: reps,
            weight,
            createdAt: DateTime.UtcNow);

        return Result<Set>.Success(set);
    }

    public Result Update(Repetitions? newReps, Weight? newWeight)
    {
        if (newReps.HasValue)
        {
            Reps = newReps.Value;
        }

        if (newWeight.HasValue)
        {
            Weight = newWeight.Value;
        }

        return Result.Success();
    }
}