using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Persistence.Repositories;

public class WorkoutRepository(FitnessDbContext context) : IWorkoutRepository
{
    public Task AddAsync(Workout workout, CancellationToken cancellationToken)
    {
        context.Workouts.AddAsync(workout, cancellationToken);
        return Task.CompletedTask;
    }

    public Task Delete(Workout workout)
    {
        context.Workouts.Remove(workout);
        return Task.CompletedTask;
    }

    public Task Update(Workout workout)
    {
        context.Workouts.Update(workout);
        return Task.CompletedTask;
    }

    public async Task<Workout?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Workouts
            .Include(x => x.Exercises).ThenInclude(e => e.Sets)
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}