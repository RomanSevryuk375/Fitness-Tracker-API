using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand : ICommand
{
    public string Login { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
