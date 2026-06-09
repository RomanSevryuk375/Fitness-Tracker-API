using FitnessTracker.Core.AggregateRoots.Workout;
using FitnessTracker.DataAccess.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.DataAccess.Configurations;

public class SetConfiguration : IEntityTypeConfiguration<Set>
{
    void IEntityTypeConfiguration<Set>.Configure(EntityTypeBuilder<Set> builder)
    {
        builder.ToTable("sets");

        builder.Property(x => x.Id)
            .HasColumnType("uuid")
            .HasConversion(
                v => ConversionHelper.StringToGuid(v),
                v => v.ToString()
            )
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.ExerciseId)
            .HasColumnType("uuid")
            .HasConversion(
                v => ConversionHelper.StringToGuid(v),
                v => v.ToString()
            );

        builder.Property(x => x.Reps);

        builder.Property(x => x.Weight);

        builder.Property(x => x.CreatedAt);

        builder.HasOne(x => x.Exercise)
            .WithMany(x => x.Sets)
            .HasForeignKey(x => x.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
