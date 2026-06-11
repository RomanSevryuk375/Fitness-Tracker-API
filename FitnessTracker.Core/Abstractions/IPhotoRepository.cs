using FitnessTracker.Core.AggregateRoots.Workouts;

namespace FitnessTracker.Core.Abstractions;

public interface IPhotoRepository
{
    Task<Photo?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Photo?> GetByFilePathAsync(string filePath, CancellationToken cancellationToken);
    Task<List<Photo>> GetByWorkoutIdAsync(Guid workoutId, CancellationToken cancellationToken);
    Task AddAsync(Photo photo, CancellationToken cancellationToken);
    Task DeleteAsync(Photo photo);
}