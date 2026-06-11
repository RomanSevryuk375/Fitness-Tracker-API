using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.Models;
using FitnessTracker.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Persistence.Repositories;

public class WorkoutRepository(FitnessDbContext context) : IWorkoutRepository
{
    public async Task<List<Workout>> GetByUserIdAsync(
        Guid userId,
        WorkoutFilter filter,
        CancellationToken cancellationToken)
    {
        var specification = new WorkoutFilterSpecification(userId, filter);

        return await SpecificationEvaluator
            .GetQuery(context.Workouts.AsNoTracking(), specification)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(
        Guid userId,
        WorkoutFilter filter,
        CancellationToken cancellationToken)
    {
        var specification = new WorkoutFilterSpecification(userId, filter);

        var query = context.Workouts.AsNoTracking().Where(specification.Criteria!);
        return await query.CountAsync(cancellationToken);
    }


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