namespace FitnessTracker.Business.DTOs;

public record UserDto
{
    public string Id { get; init; } = null!;
    public string Login { get; init; } = null!;
    public string PasswordHash { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
};
