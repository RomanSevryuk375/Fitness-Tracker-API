using FitnessTracker.Core.Entities;

namespace FitnessTracker.Core.Abstractions;

public interface IUserRepository
{
    Task<UserEntity?> GetByLoginAsync(string login, CancellationToken ct);
    Task<string> AddAsync(UserEntity user, CancellationToken ct);
    Task<string> UpdateAsync(string id, string Login, string Password, CancellationToken ct);
    Task<string> DeleteAsync(string id, CancellationToken ct);
    Task<bool> ExistsAsync(string id, CancellationToken ct);
}
