using FitnessTracker.Core.AggregateRoots.Users.ValueObjects;
using FluentValidation;

namespace FitnessTracker.Business.CQRS.Users.Commands.RegisterUser;

public sealed class RegisterUserValidator : 
    AbstractValidator<RegisterUserCommand>
{
    private const int MinPasswordLength = 6;

    public RegisterUserValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(MinPasswordLength);

        RuleFor(x => x.Login)
            .NotEmpty()
            .MaximumLength(Login.MaxLength)
            .MinimumLength(Login.MinLength)
            .Matches(Login.ValidationRegex)
            .WithMessage("Login can only contain letters, numbers, and underscores.");
    }
}
