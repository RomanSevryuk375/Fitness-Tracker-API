using FitnessTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess;

public class SystemDbContext : DbContext
{
    public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
    {
    }
    
    public DbSet<ExerciseEntity> Exercises { get; set; }
    public DbSet<PhotoEntity> Photos { get; set; }
    public DbSet<SetEntity> Sets { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<WorkoutEntity> Workouts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SystemDbContext).Assembly);
    }
}
