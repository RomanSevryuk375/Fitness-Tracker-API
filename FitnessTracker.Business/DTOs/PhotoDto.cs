namespace FitnessTracker.Business.DTOs;

public record PhotoDto
{
    public string Id { get; init; } =  null!;
    public string WorkoutId { get; init; } = null!;
    public string FilePath { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
};
