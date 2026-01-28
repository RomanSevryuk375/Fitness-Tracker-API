using FitnessTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.DataAccess.Configurations;

public class SetConfiguration : IEntityTypeConfiguration<SetEntity>
{
    void IEntityTypeConfiguration<SetEntity>.Configure(EntityTypeBuilder<SetEntity> builder)
    {
        builder.ToTable("sets");

        builder.Property(x => x.Id)
            .HasColumnType("uuid")
            .HasConversion(
                v => Guid.Parse(v),
                v => v.ToString()
            )
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.ExerciseId)
            .HasColumnType("uuid")
            .HasConversion(
                v => Guid.Parse(v),
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
