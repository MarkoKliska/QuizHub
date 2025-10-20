using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizHub.Domain.Entities;

namespace QuizHub.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions", "QuizHub");
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Text)
            .IsRequired();
        builder.Property(q => q.Type)
            .HasConversion<string>()
            .IsRequired();
        builder.Property(q => q.Points)
            .IsRequired()
            .HasDefaultValue(1);
        builder.Property(q => q.CorrectAnswer);
        builder.Property(q => q.IsDeleted)
            .HasDefaultValue(false);
        builder.HasOne(q => q.Quiz)
            .WithMany(q => q.Questions)
            .HasForeignKey(q => q.QuizId);
        builder.HasIndex(q => q.QuizId);
    }
}
