using FitnessTracker.Business.Abstractions;
using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Users;
using MediatR;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Users.Queries.LoginUser;

public sealed class LoginUserHandler(
    IUserRepository repository,
    IMyPasswordHasher myPasswordHasher,
    IJwtProvider jwtProvider) : IRequestHandler<LoginUserQuery, Result<string>>
{
    public async Task<Result<string>> Handle(
        LoginUserQuery request, 
        CancellationToken cancellationToken)
    {
        var user = await repository.GetByLoginAsync(request.Login, cancellationToken);
        if (user is null)
        {
            return Result<string>.Failure(Error.NotFound<User>(
                $"User {request.Login} not found."));
        }

        if (!myPasswordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Result<string>.Failure(Error.Conflict(
                "Password.Invalid", "Invalid password"));
        }

        return Result<string>.Success(jwtProvider.GenerateToken(user));
    }
}
