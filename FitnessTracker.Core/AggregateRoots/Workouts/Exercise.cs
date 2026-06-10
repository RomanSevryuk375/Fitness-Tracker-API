using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Workouts;

public sealed class Exercise : Entity
{
    private const int SetLimit = 50;

    private readonly List<Set> _sets = [];

    public Guid WorkoutId { get; private set; }
    public ExerciseName Name { get; private set; }

    public IReadOnlyCollection<Set> Sets  => _sets.AsReadOnly();
    public Workout? Workout { get; private set; }

    private Exercise(
        Guid id,
        Guid workoutId,
        ExerciseName name,
        DateTime createdAt)
    {
        Id = id;
        WorkoutId = workoutId;
        Name = name;
        CreatedAt = createdAt;
    }

    public static Result<Exercise> Create(Guid workoutId, ExerciseName name)
    {
        var exercise = new Exercise(
            id: Guid.NewGuid(),
            workoutId: workoutId,
            name: name,
            createdAt: DateTime.UtcNow);

        return Result<Exercise>.Success(exercise);
    }

    public Result AddSet(Set set)
    {
        if (_sets.Count >= SetLimit)
        {
            return Result.Failure(Error.Validation<Exercise>(
                "Cannot add more than 50 sets to a single exercise."));
        }

        _sets.Add(set);
        return Result.Success();
    }

    public Result RemoveSet(Guid setId)
    {
        var set = _sets.FirstOrDefault(s => s.Id == setId);
        if (set is null)
        {
            return Result.Failure(Error.NotFound<Exercise>(
                "Set not found in this exercise."));
        }

        _sets.Remove(set);
        return Result.Success();
    }
}
