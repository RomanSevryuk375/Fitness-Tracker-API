using FitnessTracker.Core.AggregateRoots.MediaAttachments;

namespace FitnessTracker.Core.Abstractions;

public interface IPhotoRepository
{
    Task<Photo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Photo?> GetByFilePathAsync(string filePath, CancellationToken cancellationToken = default);
    Task<List<Photo>> GetByWorkoutIdAsync(Guid workoutId, CancellationToken cancellationToken = default);
    Task AddAsync(Photo photo, CancellationToken cancellationToken = default);
    Task DeleteAsync(Photo photo);
}