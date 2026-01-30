using FitnessTracker.Core.Entities;

namespace FitnessTracker.Business.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(UserEntity user);
}
