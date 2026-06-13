using FitnessTracker.Core.AggregateRoots.Users;
using Shared.Result;

namespace FitnessTracker.Core.Abstractions;

public interface IUserRepository
{
    Task<Guid> AddAsync(User user, CancellationToken cancellationToken);
    Task Delete(User user);
    Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken);
    Task<PagedResult<User>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task Update(User user);
}