using FitnessTracker.Core.Entities;
using FitnessTracker.DataAccess.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.DataAccess.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<PhotoEntity>
{
    void IEntityTypeConfiguration<PhotoEntity>.Configure(EntityTypeBuilder<PhotoEntity> builder)
    {
        builder.ToTable("photos");

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

        builder.Property(x => x.FilePath);

        builder.Property(x => x.CreatedAt);

        builder.HasOne(x => x.Workout)
            .WithMany(x => x.Photos)
            .HasForeignKey(x => x.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
