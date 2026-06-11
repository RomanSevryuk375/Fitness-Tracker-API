using FitnessTracker.Business.Abstractions;
using Shared.Result;

namespace FitnessTracker.Business.CQRS.Users.Queries.LoginUser;

public sealed record LoginUserQuery : IQuery<Result<string>>
{
    public string Login { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
