using FitnessTracker.Core.AggregateRoots.Workouts;

namespace FitnessTracker.Core.Abstractions;

public interface IWorkoutRepository
{
    Task AddAsync(Workout workout, CancellationToken cancellationToken);
    Task Delete(Workout workout);
    Task<Workout?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task Update(Workout workout);
}