using FitnessTracker.Core.AggregateRoots.Workout;
using FitnessTracker.Core.Models;

namespace FitnessTracker.Core.Abstractions;

public interface IWorkoutRepository
{
    Task<Workout?> GetByIdAsync(string id, CancellationToken ct);
    Task<List<Workout>> GetByUserIdAsync(string userId, WorkoutFilter filter, CancellationToken ct);
    Task<int> GetCountAsync(string userId, WorkoutFilter filter, CancellationToken ct);
    Task<string> AddAsync(Workout workout, CancellationToken ct);
    Task<string> UpdateAsync(string id, WorkoutUpdateModel model, CancellationToken ct);
    Task<string> DeleteAsync(string id, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
