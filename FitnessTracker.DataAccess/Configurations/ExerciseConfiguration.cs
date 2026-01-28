using FitnessTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.DataAccess.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<ExerciseEntity>
{
    void IEntityTypeConfiguration<ExerciseEntity>.Configure(EntityTypeBuilder<ExerciseEntity> builder)
    {
        builder.ToTable("exercises");

        builder.Property(x => x.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.WorkoutId)
            .HasColumnType("uuid");

        builder.Property(x => x.Name);

        builder.Property(x => x.CreatedAt);

        builder.HasOne(x => x.Workout)
            .WithMany(x => x.Exercises)
            .HasForeignKey(x => x.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Sets)
            .WithOne(x => x.Exercise)
            .HasForeignKey(x => x.ExerciseId) 
            .OnDelete(DeleteBehavior.Cascade);
    }
}
