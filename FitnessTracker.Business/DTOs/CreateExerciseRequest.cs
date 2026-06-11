namespace FitnessTracker.Business.DTOs;

public record CreateExerciseRequest
{
    public string Name { get; init; } = null!;
};
