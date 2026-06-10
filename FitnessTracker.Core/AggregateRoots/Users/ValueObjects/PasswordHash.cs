using Shared.Result;
using System.Text.RegularExpressions;

namespace FitnessTracker.Core.AggregateRoots.Users.ValueObjects;

public readonly record struct PasswordHash
{
    public const int HashLength = 60;

    public static readonly Regex BCryptRegex = new(@"^\$2[abxy]\$\d{2}\$[./A-Za-z0-9]{53}$", 
        RegexOptions.Compiled);

    public string Value { get; }

    public PasswordHash(string passwordHash)
    {
        Value = passwordHash;
    }

    public static Result<PasswordHash> Create(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
        {
            return Result<PasswordHash>.Failure(Error.Validation<PasswordHash>("Password hash cannot be empty."));
        }

        if (hash.Length != HashLength)
        {
            return Result<PasswordHash>.Failure(Error.Validation<PasswordHash>(
                $"Invalid hash length. Expected {HashLength}, got {hash.Length}."));
        }

        if (!BCryptRegex.IsMatch(hash))
        {
            return Result<PasswordHash>.Failure(Error.Validation<PasswordHash>(
                "Invalid password hash format. Expected a valid BCrypt hash."));
        }

        return Result<PasswordHash>.Success(new PasswordHash(hash));
    }

    public static implicit operator string(PasswordHash hash) => hash.Value;

    public override string ToString() => Value.ToString();
}
