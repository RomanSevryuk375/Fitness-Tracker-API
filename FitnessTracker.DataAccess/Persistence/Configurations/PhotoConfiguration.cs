using FitnessTracker.Core.AggregateRoots.MediaAttachments;
using FitnessTracker.Core.AggregateRoots.MediaAttachments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.DataAccess.Persistence.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("photos");

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.WorkoutId)
            .HasColumnName("workout_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.FilePath)
            .HasColumnName("file_path")
            .HasConversion(
                vo => vo.Value,
                dbValue => FilePath.Create(dbValue).Value
            )
            .HasMaxLength(FilePath.MaxLength)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasOne(x => x.Workout)
            .WithMany(x => x.Photos)
            .HasForeignKey(x => x.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
