namespace FitnessTracker.Business.DTOs;

public record RegisterRequest
{
    public string Login { get; init; } = null!;
    public string Password { get; init; } = null!;
};
