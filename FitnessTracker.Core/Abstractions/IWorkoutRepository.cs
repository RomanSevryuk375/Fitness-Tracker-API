using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.Models;

namespace FitnessTracker.Core.Abstractions;

public interface IWorkoutRepository
{
    Task AddAsync(Workout workout, CancellationToken cancellationToken);
    Task Delete(Workout workout);
    Task<Workout?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Workout>> GetByUserIdAsync(
        Guid userId, 
        WorkoutFilter filter, 
        CancellationToken cancellationToken);
    Task<int> GetCountAsync(Guid userId, WorkoutFilter filter, CancellationToken cancellationToken);
    Task Update(Workout workout);
}