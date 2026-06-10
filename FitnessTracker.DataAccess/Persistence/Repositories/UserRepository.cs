using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.AggregateRoots.Users;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace FitnessTracker.DataAccess.Persistence.Repositories;

public class UserRepository(FitnessDbContext context) : IUserRepository
{
    public async Task<PagedResult<User>> GetAll(int pageNumber, int pageSize)
    {
        return await context.Users
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ToPagedResultAsync(pageNumber, pageSize);
    }

    public async Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }

    public async Task<Guid> AddAsync(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);

        return user.Id;
    }

    public async Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == userId, cancellationToken);
    }

    public Task Update(User user)
    {
        context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task Delete(User user)
    {
        context.Users.Remove(user);
        return Task.CompletedTask;
    }
}
