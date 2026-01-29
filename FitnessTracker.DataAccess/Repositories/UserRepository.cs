using FitnessTracker.Core.Abstractions;
using FitnessTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SystemDbContext _context;

    public UserRepository(SystemDbContext context)
    {
        _context = context;
    }
    public async Task<string> AddAsync(UserEntity user, CancellationToken ct)
    {
        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);

        return user.Id;
    }

    public async Task<string> DeleteAsync(string id, CancellationToken ct)
    {
        var user = await _context.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync(ct);

        return id;
    }

    public async Task<bool> ExistsAsync(string id, CancellationToken ct)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == id, ct);
    }

    public async Task<UserEntity?> GetByLoginAsync(string login, CancellationToken ct)
    {
        return await _context.Users.AsNoTracking()
            .Where(u => u.Login == login)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<string> UpdateAsync(string id, string login, string passwordHash, CancellationToken ct)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct)
            ?? throw new ArgumentException("User not found");

        if (!string.IsNullOrWhiteSpace(login)) 
            entity.SetLogin(login);

        if (!string.IsNullOrWhiteSpace(passwordHash))
            entity.SetPasswordHash(passwordHash);            

        await _context.SaveChangesAsync(ct);

        return entity.Id;
    }
}
