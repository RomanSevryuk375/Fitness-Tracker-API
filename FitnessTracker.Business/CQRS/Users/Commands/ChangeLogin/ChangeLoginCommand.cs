using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Users.Commands.ChangeLogin;

public sealed record ChangeLoginCommand : ICommand
{
    public Guid UserId { get; init; }
    public string NewLogin { get; init; } = string.Empty;
}
