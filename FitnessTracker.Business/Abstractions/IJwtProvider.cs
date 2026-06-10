using FitnessTracker.Core.AggregateRoots.Users;

namespace FitnessTracker.Business.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
