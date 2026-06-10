using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.Models;

namespace FitnessTracker.Core.Specifications;

public sealed class WorkoutFilterSpecification : BaseSpecification<Workout>
{
    public WorkoutFilterSpecification(Guid userId, WorkoutFilter filter)
        : base(w =>
            w.UserId == userId &&
            (filter.Types == null || !filter.Types.Any() || filter.Types.Contains(w.Type)) &&
            (!filter.DateFrom.HasValue || w.WorkoutDate >= filter.DateFrom.Value) &&
            (!filter.DateTo.HasValue || w.WorkoutDate <= filter.DateTo.Value) &&
            (!filter.MinDuration.HasValue || w.Duration >= filter.MinDuration.Value) &&
            (!filter.MaxDuration.HasValue || w.Duration <= filter.MaxDuration.Value))
    {
        AddInclude(w => w.Photos);
        AddInclude(w => w.Exercises);
        AddInclude($"{nameof(Workout.Exercises)}.{nameof(Exercise.Sets)}");

        if (!string.IsNullOrWhiteSpace(filter.SortBy))
        {
            switch (filter.SortBy.ToLower().Trim())
            {
                case "duration":
                    if (filter.IsDescending)
                    {
                        ApplyOrderByDescending(w => w.Duration);
                    }
                    else
                    {
                        ApplyOrderBy(w => w.Duration);
                    }

                    break;
                case "caloriesburned":
                    if (filter.IsDescending)
                    {
                        ApplyOrderByDescending(w => w.CaloriesBurned);
                    }
                    else
                    {
                        ApplyOrderBy(w => w.CaloriesBurned);
                    }

                    break;
                default:
                    if (filter.IsDescending)
                    {
                        ApplyOrderByDescending(w => w.WorkoutDate);
                    }
                    else
                    {
                        ApplyOrderBy(w => w.WorkoutDate);
                    }

                    break;
            }
        }
        else
        {
            ApplyOrderByDescending(w => w.WorkoutDate);
        }

        ApplyPaging((filter.Page - 1) * filter.Limit, filter.Limit);
    }
}