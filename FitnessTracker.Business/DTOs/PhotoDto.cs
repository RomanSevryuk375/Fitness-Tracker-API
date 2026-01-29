namespace FitnessTracker.Business.DTOs;

public record PhotoDto
(
    string Id,
    string WorkoutId,
    string FilePath,
    DateTime CreatedAt
);
