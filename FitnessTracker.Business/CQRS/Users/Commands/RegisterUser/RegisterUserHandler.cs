using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Users;
using FitnessTracker.Core.AggregateRoots.Users.ValueObjects;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Users.Commands.RegisterUser;

public sealed class RegisterUserHandler(
    IUserRepository repository,
    IMyPasswordHasher myPasswordHasher) : IRequestHandler<RegisterUserCommand, Result>
{
    public async Task<Result> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var loginResult = Login.Create(request.Login);
        if (loginResult.IsFailure)
        {
            return Result.Failure(loginResult.Error);
        }

        var login = loginResult.Value;
        var existingUser = await repository.GetByLoginAsync(login, cancellationToken);
        if (existingUser is not null)
        {
            return Result.Failure(Error.Conflict<User>($"User '{login}' already exists."));
        }

        var passwordResult = PasswordHash.Create(myPasswordHasher.Generate(request.Password));
        if (passwordResult.IsFailure)
        {
            return Result.Failure(passwordResult.Error);
        }

        var userResult = User.Create(login, passwordResult.Value);
        if (userResult.IsFailure)
        {
            return Result.Failure(userResult.Error);
        }

        await repository.AddAsync(userResult.Value, cancellationToken);

        return Result.Success();
    }
}
