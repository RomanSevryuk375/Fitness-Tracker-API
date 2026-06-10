using FitnessTracker.Core.AggregateRoots.Users;
using FitnessTracker.Core.AggregateRoots.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessTracker.DataAccess.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(t => t.Id)
            .HasColumnName("id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.Login)
            .HasColumnName("login")
            .HasConversion(
                vb => vb.Value,
                dbValue => Login.Create(dbValue).Value
            )
            .HasMaxLength(Login.MaxLength)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasColumnName("password_hash")
            .HasConversion(
                vb => vb.Value,
                dbValue => PasswordHash.Create(dbValue).Value
            )
            .HasMaxLength(PasswordHash.HashLength)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired(); 

        builder.HasMany(x => x.Workouts)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
