using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizHub.Domain.Entities;

namespace QuizHub.Infrastructure.Persistence.Configurations;

public class QuizAttemptAnswerConfiguration : IEntityTypeConfiguration<QuizAttemptAnswer>
{
    public void Configure(EntityTypeBuilder<QuizAttemptAnswer> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.QuizAttemptId)
            .IsRequired();
        builder.Property(a => a.QuestionId)
            .IsRequired();
        builder.Property(a => a.SelectedOptionIds)
            .HasConversion(
                v => v == null ? null : string.Join(',', v),
                v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(Guid.Parse).ToList()
            );
        builder.Property(a => a.TextAnswer)
            .HasMaxLength(500);
        builder.Property(a => a.IsCorrect)
            .IsRequired();
        builder.Property(a => a.PointsAwarded)
            .IsRequired();
        builder.HasOne(a => a.QuizAttempt)
            .WithMany()
            .HasForeignKey(a => a.QuizAttemptId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a => a.Question)
            .WithMany()
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
