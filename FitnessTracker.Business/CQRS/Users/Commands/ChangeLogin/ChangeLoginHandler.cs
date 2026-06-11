using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Users;
using FitnessTracker.Core.AggregateRoots.Users.ValueObjects;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Users.Commands.ChangeLogin;

public sealed class ChangeLoginHandler(
    IUserRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangeLoginCommand, Result>
{
    public async Task<Result> Handle(
        ChangeLoginCommand request,
        CancellationToken cancellationToken)
    {
        var loginResult = Login.Create(request.NewLogin);
        if (loginResult.IsFailure)
        {
            return Result.Failure(loginResult.Error);
        }

        var user = await repository.GetByIdAsync(
            request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(Error.NotFound<User>(
                $"User {request.UserId} not found."));
        }

        if (user.Login.Value == request.NewLogin)
        {
            return Result.Success();
        }

        var existingUser = await repository.GetByLoginAsync(
            loginResult.Value, cancellationToken);
        if (existingUser is not null)
        {
            return Result.Failure(Error.Conflict<User>(
                $"Login '{request.NewLogin}' is already taken."));
        }

        var updateResult = user.UpdateLogin(request.NewLogin);
        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        await repository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}