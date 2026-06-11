using FitnessTracker.Core.Abstractions;

namespace FitnessTracker.DataAccess.Extensions;

public sealed class UnitOfWork(FitnessDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
