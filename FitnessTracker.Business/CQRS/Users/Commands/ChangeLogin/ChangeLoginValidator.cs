using FitnessTracker.Core.AggregateRoots.Users.ValueObjects;
using FluentValidation;

namespace FitnessTracker.Business.CQRS.Users.Commands.ChangeLogin;

public sealed class ChangeLoginValidator : AbstractValidator<ChangeLoginCommand>
{
    public ChangeLoginValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.NewLogin)
            .NotEmpty()
            .MaximumLength(Login.MaxLength)
            .MinimumLength(Login.MinLength)
            .Matches(Login.ValidationRegex)
            .WithMessage("Login can only contain letters, numbers, and underscores.");
    }
}