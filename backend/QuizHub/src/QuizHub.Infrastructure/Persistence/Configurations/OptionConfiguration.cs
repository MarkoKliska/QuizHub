using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizHub.Domain.Entities;

namespace QuizHub.Infrastructure.Persistence.Configurations;

public class OptionConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.ToTable("Options", "QuizHub");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Text)
            .IsRequired();
        builder.Property(o => o.IsCorrect)
            .IsRequired();
        builder.HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId);
        builder.HasIndex(o => o.QuestionId);
    }
}
