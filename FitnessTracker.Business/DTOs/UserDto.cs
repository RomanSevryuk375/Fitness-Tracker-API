namespace FitnessTracker.Business.DTOs;

public record UserDto
(
    string Id,
    string Login,
    string PasswordHash,
    DateTime CreatedAt
);
