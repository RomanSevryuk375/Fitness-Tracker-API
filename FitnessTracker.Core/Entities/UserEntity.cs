using FitnessTracker.Core.Abstractions;
using Shared.Result;

namespace FitnessTracker.Core.Entities;

public class UserEntity : IDocument
{
    public Guid Id { get; set; } 
    public string Login { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; set; } 
    public List<Workout> Workouts { get; private set; } = [];

    private UserEntity(
        Guid id, 
        string login, 
        string passwordHash, 
        DateTime createdAt)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
    }

    public static Result<UserEntity> Create(string login, string passwordHash)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(login))
        {
            errors.Add("Login can not be empty");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            errors.Add("PasswordHash can not be empty");
        }

        if (errors.Count != 0)
        {
            return Result<UserEntity>.Failure(Error.Validation(
                $"{nameof(UserEntity)}.Invalid",
                string.Join("; ", errors)));
        }

        var user = new UserEntity(
            Guid.NewGuid(),
            login.Trim(), 
            passwordHash.Trim(), 
            DateTime.UtcNow);

        return Result<UserEntity>.Success(user);
    }

    public Result SetLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            return Result.Failure(Error.Validation(
                $"{nameof(UserEntity)}.Invalid",
                "Login can not be empty"));
        }

        Login = login.Trim();

        return Result.Success();
    }

    public Result SetPasswordHash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return Result.Failure(Error.Validation(
                $"{nameof(UserEntity)}.Invalid",
                "Password can not be empty"));
        }

        PasswordHash = password.Trim();

        return Result.Success();
    }
}
