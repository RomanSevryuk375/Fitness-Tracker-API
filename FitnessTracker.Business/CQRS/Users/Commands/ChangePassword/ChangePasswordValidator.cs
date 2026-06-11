using FluentValidation;

namespace FitnessTracker.Business.CQRS.Users.Commands.ChangePassword;

public sealed class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    private const int MinPasswordLength = 6;

    public ChangePasswordValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.OldPassword)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(MinPasswordLength)
            .NotEqual(x => x.OldPassword)
            .WithMessage("New password must be different from the old password.");
    }
}