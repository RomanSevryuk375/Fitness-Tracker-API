using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Users;
using FitnessTracker.Core.AggregateRoots.Users.ValueObjects;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Users.Commands.ChangePassword;

public sealed class ChangePasswordHandler(
    IUserRepository repository,
    IMyPasswordHasher myPasswordHasher,
    IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(
        ChangePasswordCommand request, 
        CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(Error.NotFound<User>(
                $"User {request.UserId} not found."));
        }

        if (!myPasswordHasher.Verify(request.OldPassword, user.PasswordHash.Value))
        {
            return Result.Failure(Error.Validation<User>(
                "Incorrect old password."));
        }

        var newHashString = myPasswordHasher.Generate(request.NewPassword);
        var passwordResult = PasswordHash.Create(newHashString);
        if (passwordResult.IsFailure)
        {
            return Result.Failure(passwordResult.Error);
        }


        var updateResult = user.SetPasswordHash(newHashString);
        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        await repository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}