using FitnessTracker.Core.Entities;
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
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.WorkoutId)
            .HasColumnType("uuid");

        builder.Property(x => x.FilePath);

        builder.Property(x => x.CreatedAt);

        builder.HasOne(x => x.Workout)
            .WithMany(x => x.Photos)
            .HasForeignKey(x => x.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
