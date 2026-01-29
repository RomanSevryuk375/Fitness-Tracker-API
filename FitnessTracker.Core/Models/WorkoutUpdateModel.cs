using FitnessTracker.Core.Enums;

namespace FitnessTracker.Core.Models;

public record WorkoutUpdateModel
(
    string? Title,
    WorkoutType? Type,
    TimeSpan? Duration,
    double? CaloriesBurned,
    DateTime? WorkoutDate
);