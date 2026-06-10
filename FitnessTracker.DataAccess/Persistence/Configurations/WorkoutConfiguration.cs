using FitnessTracker.Core.AggregateRoots.Workouts;
using FitnessTracker.Core.AggregateRoots.Workouts.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.DataAccess.Persistence.Configurations;

internal class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("workouts");

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasConversion(
                vb => vb.Value,
                dbValue => WorkoutTitle.Create(dbValue).Value
            )
            .HasMaxLength(WorkoutTitle.MaxLength)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.Duration)
            .HasColumnName("duration")
            .HasConversion(
                vb => vb.Value,
                dbValue => WorkoutDuration.Create(dbValue).Value
            )
            .IsRequired(); 

        builder.Property(x => x.CaloriesBurned)
            .HasColumnName("calories")
            .HasConversion(
                vb => vb.Value,
                dbValue => Calories.Create(dbValue).Value
            )
            .IsRequired();

        builder.Property(x => x.WorkoutDate)
            .HasColumnName("workout_date")
            .HasConversion(
                vb => vb.Value,
                dbValue => WorkoutDate.Restore(dbValue)
            )
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

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
