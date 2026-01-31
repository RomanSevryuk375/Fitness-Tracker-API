namespace FitnessTracker.Business.DTOs;

public record SetDto
{
    public int Reps { get; init; }
    public double Weight { get; init; }
}