using Shared.Result;
using System.Text.RegularExpressions;

namespace FitnessTracker.Core.AggregateRoots.Users.ValueObjects;

public readonly record struct Login
{
    public const int MinLength = 5;
    public const int MaxLength = 128;

    public static readonly Regex ValidationRegex = new(@"^[a-zA-Z0-9_]+$", RegexOptions.Compiled);

    public string Value { get; }

    private Login(string login)
    {
        Value = login;
    }

    public static Result<Login> Create(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            return Result<Login>.Failure(Error.Validation<Login>(
                "Login can not be empty"));
        }

        var trimmedLogin = login.Trim();
            
        if (trimmedLogin.Length < MinLength || trimmedLogin.Length > MaxLength)
        {
            return Result<Login>.Failure(Error.Validation<Login>(
                $"Login must be between {MinLength} and {MaxLength} characters."));
        }

        if (!ValidationRegex.IsMatch(trimmedLogin))
        {
            return Result<Login>.Failure(Error.Validation<Login>(
                "Login can only contain letters, numbers, and underscores."));
        }

        return Result<Login>.Success(new Login(login));
    }

    public static implicit operator string(Login login) => login.Value;

    public override string ToString() => Value.ToString();
}
