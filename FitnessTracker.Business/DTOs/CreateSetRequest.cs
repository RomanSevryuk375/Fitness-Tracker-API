namespace FitnessTracker.Business.DTOs;

public record CreateSetRequest
{
    public int Reps { get; init; }
    public double Weight { get; init; }
};
