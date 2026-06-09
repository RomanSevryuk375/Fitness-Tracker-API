using FitnessTracker.Core.AggregateRoots.Workout;
using FitnessTracker.DataAccess.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.DataAccess.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    void IEntityTypeConfiguration<Exercise>.Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.ToTable("exercises");

        builder.Property(x => x.Id)
            .HasColumnType("uuid")
            .HasConversion(
                v => ConversionHelper.StringToGuid(v),
                v => v.ToString()
            )
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.WorkoutId)
            .HasColumnType("uuid")
            .HasConversion(
                v => ConversionHelper.StringToGuid(v),
                v => v.ToString()
            );

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
