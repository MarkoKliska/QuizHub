using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizHub.Domain.Entities;

namespace QuizHub.Infrastructure.Persistence.Configurations;

public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
{
    public void Configure(EntityTypeBuilder<QuizAttempt> builder)
    {
        builder.ToTable("QuizAttempts", "QuizHub");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.StartTime)
            .IsRequired();
        builder.Property(a => a.Score)
            .IsRequired(false);
        builder.Property(a => a.Percentage)
            .IsRequired(false);
        builder.Property(a => a.IsCompleted)
            .IsRequired();
        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId);
        builder.HasOne(a => a.Quiz)
            .WithMany()
            .HasForeignKey(a => a.QuizId);
        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => a.QuizId);
        builder.HasIndex(a => new { a.Score, a.EndTime });
    }
}
