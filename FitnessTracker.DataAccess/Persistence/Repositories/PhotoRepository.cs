using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Persistence.Repositories;

public class PhotoRepository(FitnessDbContext context) : IPhotoRepository
{
    public async Task<Photo?> GetByIdAsync(
        Guid id, CancellationToken cancellationToken)
    {
        return await context.Photos
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Photo?> GetByFilePathAsync(
        string filePath, CancellationToken cancellationToken)
    {
        return await context.Photos
            .FirstOrDefaultAsync(p => p.FilePath == filePath, cancellationToken);
    }

    public async Task<List<Photo>> GetByWorkoutIdAsync(
        Guid workoutId, CancellationToken cancellationToken)
    {
        return await context.Photos
            .AsNoTracking()
            .Where(p => p.WorkoutId == workoutId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Photo photo, CancellationToken cancellationToken)
    {
        await context.Photos.AddAsync(photo, cancellationToken);
    }

    public Task DeleteAsync(Photo photo)
    {
        context.Photos.Remove(photo);
        return Task.CompletedTask;
    }
}