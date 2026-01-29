using FitnessTracker.Core.Enums;

namespace FitnessTracker.Core.Models;

public record WorkoutFilter
(
    IEnumerable<WorkoutType>? Types, 
    DateTime? DateFrom,              
    DateTime? DateTo,               
    TimeSpan? MinDuration,           
    TimeSpan? MaxDuration,
    string? SortBy,
    int Page,
    int Limit,
    bool IsDescending
);
