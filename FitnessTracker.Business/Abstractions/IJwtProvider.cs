using FitnessTracker.Core.AggregateRoots.User;

namespace FitnessTracker.Business.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
