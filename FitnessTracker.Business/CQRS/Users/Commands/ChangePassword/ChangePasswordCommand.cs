using FitnessTracker.Business.Abstractions;

namespace FitnessTracker.Business.CQRS.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand : ICommand
{
    public Guid UserId { get; init; }
    public string OldPassword { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
}
