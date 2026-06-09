using FitnessTracker.Core.AggregateRoots.User;

namespace FitnessTracker.Core.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByLoginAsync(string login, CancellationToken ct);
    Task<string> AddAsync(User user, CancellationToken ct);
    Task<string> UpdateAsync(string id, string login, string password, CancellationToken ct);
    Task<string> DeleteAsync(string id, CancellationToken ct);
    Task<bool> ExistsAsync(string id, CancellationToken ct);
}
