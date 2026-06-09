using FitnessTracker.Core.AggregateRoots.MediaAttachment;
using FitnessTracker.Core.AggregateRoots.User;
using FitnessTracker.Core.AggregateRoots.Workout;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess;

public class SystemDbContext : DbContext
{
    public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
    {
    }
    
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Set> Sets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Workout> Workouts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SystemDbContext).Assembly);
    }
}
