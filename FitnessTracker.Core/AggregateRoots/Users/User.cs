using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Users.ValueObjects;
using FitnessTracker.Core.AggregateRoots.Workouts;
using Shared.Result;

namespace FitnessTracker.Core.AggregateRoots.Users;

public sealed class User : Entity, IAggregateRoot
{
    public Login Login { get; private set; }
    public PasswordHash PasswordHash { get; private set; }

    public List<Workout> Workouts { get; private set; } = [];

    private User(
        Guid id,
        Login login,
        PasswordHash passwordHash,
        DateTime createdAt)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
    }

    public static Result<User> Create(Login login, PasswordHash passwordHash)
    {
        var user = new User(
            id: Guid.NewGuid(),
            login,
            passwordHash,
            createdAt: DateTime.UtcNow);

        return Result<User>.Success(user);
    }

    public Result UpdateLogin(string newLogin)
    {
        var loginResult = Login.Create(newLogin);
        if (loginResult.IsFailure)
        {
            return Result.Failure(loginResult.Error);
        }

        Login = loginResult.Value;

        return Result.Success();
    }

    public Result SetPasswordHash(string newHash)
    {
        var passwordResult = PasswordHash.Create(newHash);
        if (passwordResult.IsFailure)
        {
            return Result.Failure(passwordResult.Error);
        }

        PasswordHash = passwordResult.Value;

        return Result.Success();
    }
}
