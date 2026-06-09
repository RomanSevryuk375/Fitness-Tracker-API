using FitnessTracker.Core.AggregateRoots.Workout;
using FitnessTracker.DataAccess.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.DataAccess.Configurations;

internal class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    void IEntityTypeConfiguration<Workout>.Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("workouts");

        builder.Property(x => x.Id)
            .HasColumnType("uuid")
            .HasConversion(
                v => ConversionHelper.StringToGuid(v),
                v => v.ToString()
            )
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.UserId)
            .HasColumnType("uuid")
            .HasConversion(
                v => ConversionHelper.StringToGuid(v),
                v => v.ToString()
            );

        builder.Property(x => x.Title);

        builder.Property(x => x.Type)
            .HasConversion<string>();

        builder.Property(x => x.Duration);

        builder.Property(x => x.CaloriesBurned);

        builder.Property(x => x.WorkoutDate);

        builder.Property(x => x.CreatedAt);


        builder.HasMany(x => x.Exercises)
            .WithOne(x => x.Workout)
            .HasForeignKey(x => x.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Photos)
           .WithOne(x => x.Workout)
           .HasForeignKey(x => x.WorkoutId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Workouts)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
