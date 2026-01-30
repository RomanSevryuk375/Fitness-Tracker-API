namespace FitnessTracker.Business.DTOs;

public record ExerciseDto
{
    public string Name { get; init; } = null!;
    public List<SetDto> Sets { get; init; } = [];
}
