using FitnessTracker.Business.DTOs;
using FitnessTracker.Core.Models;

namespace FitnessTracker.Business.Abstractions;

public interface IWorkoutService
{
    Task<WorkoutDto> CreateAsync(string userId, CreateWorkoutRequest request, CancellationToken ct);
    Task<string> DeleteWorkout(string userId, string id, CancellationToken ct);
    Task<WorkoutDto?> GetByIdAsync(string userId, string workoutId, CancellationToken ct);
    Task<int> GetCountAsync(string userId, WorkoutFilter filter, CancellationToken ct);
    Task<List<WorkoutDto>> GetUserWorkoutsAsync(string userId, WorkoutFilter filter, CancellationToken ct);
    Task<string> UpdateWorkout(string userId, string id, WorkoutUpdateModel model, CancellationToken ct);
}