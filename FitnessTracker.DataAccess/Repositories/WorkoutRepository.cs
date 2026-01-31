using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.Entities;
using FitnessTracker.Core.Exceptions;
using FitnessTracker.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly SystemDbContext _context;

    public WorkoutRepository(SystemDbContext context)
    {
        _context = context;
    }

    private static IQueryable<WorkoutEntity> ApplyFilter(IQueryable<WorkoutEntity> query, WorkoutFilter filter)
    {
        if (filter.Types is not null && filter.Types.Any())
            query = query.Where(o => filter.Types.Contains(o.Type));

        if (filter.DateFrom is not null)
            query = query.Where(o => o.WorkoutDate >= filter.DateFrom);

        if (filter.DateTo is not null)
            query = query.Where(o => o.WorkoutDate <= filter.DateTo);

        if (filter.MinDuration is not null)
            query = query.Where(o => o.Duration >= filter.MinDuration);

        if (filter.MaxDuration is not null)
            query = query.Where(o => o.Duration <= filter.MaxDuration);

        return query;
    }

    public async Task<string> AddAsync(WorkoutEntity workout, CancellationToken ct)
    {
        await _context.Workouts.AddAsync(workout, ct);
        await _context.SaveChangesAsync(ct);

        return workout.Id;
    }

    public async Task<string> DeleteAsync(string id, CancellationToken ct)
    {
        await _context.Workouts.Where(x => x.Id == id)
            .ExecuteDeleteAsync(ct);

        return id;
    }

    public async Task<WorkoutEntity?> GetByIdAsync(string id, CancellationToken ct)
    {
        return await _context.Workouts
                        .Where(x => x.Id == id)
                        .Include(x => x.Exercises)
                            .ThenInclude(e => e.Sets)
                        .Include(x => x.Photos)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ct);
    }

    public async Task<int> GetCountAsync(string userId, WorkoutFilter filter, CancellationToken ct)
    {
        var query = _context.Workouts
                        .Where(x => x.UserId == userId)
                        .AsNoTracking();

        query = ApplyFilter(query, filter);

        return await query.CountAsync(ct);
    }

    public async Task<List<WorkoutEntity>> GetByUserIdAsync(string userId, WorkoutFilter filter, CancellationToken ct)
    {
        var query = _context.Workouts
                        .Where(x => x.UserId == userId)
                        .Include(x => x.Exercises)
                            .ThenInclude(e => e.Sets)
                        .Include(x => x.Photos)
                        .AsNoTracking();

        query = ApplyFilter(query, filter);

        query = filter.SortBy?.ToLower().Trim() switch
        {
            "duration" => filter.IsDescending
                ? query.OrderByDescending(x => x.Duration)
                : query.OrderBy(x => x.Duration),

            "caloriesburned" => filter.IsDescending
                ? query.OrderByDescending(x => x.CaloriesBurned)
                : query.OrderBy(x => x.CaloriesBurned),

            _ => filter.IsDescending
                ? query.OrderByDescending(x => x.WorkoutDate)
                : query.OrderBy(x => x.WorkoutDate)
        };

        return await query
            .Skip((filter.Page - 1) * filter.Limit)
            .Take(filter.Limit)
            .ToListAsync(ct);
    }

    public async Task<string> UpdateAsync(string id, WorkoutUpdateModel model, CancellationToken ct)
    {
        var entity = await _context.Workouts.FirstOrDefaultAsync(x => x.Id == id, ct)
        ?? throw new NotFoundException("Workout not found");

        entity.Update(
            model.Title,
            model.Type,
            model.Duration,
            model.CaloriesBurned,
            model.WorkoutDate);

        await _context.SaveChangesAsync(ct);

        return entity.Id;
    }

    public async Task SaveChangesAsync(CancellationToken ct) => await _context.SaveChangesAsync(ct);
}
