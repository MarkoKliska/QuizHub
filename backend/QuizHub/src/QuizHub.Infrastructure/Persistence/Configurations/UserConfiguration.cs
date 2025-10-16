using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizHub.Domain.Entities;

namespace QuizHub.Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "QuizHub");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.PasswordHash)
            .IsRequired();
        builder.Property(u => u.ProfileImage)
            .HasColumnType("bytea");
        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("NOW()");
        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(u => u.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
    }
}