using FitnessTracker.Business.DTOs;
using FitnessTracker.Core.Models;

namespace FitnessTracker.Business.Abstractions;

public interface IWorkoutService
{
    Task<WorkoutDto> CreateAsync(string userId, CreateWorkoutRequest request, List<FileModel> photos, CancellationToken ct);
    Task<string> DeleteWorkout(string userId, string id, CancellationToken ct);
    Task<WorkoutDto?> GetByIdAsync(string userId, string workoutId, CancellationToken ct);
    Task<int> GetCountAsync(string userId, WorkoutFilter filter, CancellationToken ct);
    Task<List<WorkoutDto>> GetUserWorkoutsAsync(string userId, WorkoutFilter filter, CancellationToken ct);
    Task<string> UpdateWorkout(string userId, string id, WorkoutUpdateModel model, CancellationToken ct);
    Task<(Stream FileStream, string ContentType)> GetPhotoAsync(string userId, string workoutId, string fileName, CancellationToken ct);
}