namespace FitnessTracker.Business.DTOs;

public record LoginRequest
{
    public string Login { get; init; } = null!;
    public string Password { get; init; } = null!;
};
