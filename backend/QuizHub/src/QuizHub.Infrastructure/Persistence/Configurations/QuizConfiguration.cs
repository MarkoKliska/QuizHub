using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizHub.Domain.Entities;

namespace QuizHub.Infrastructure.Persistence.Configurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizzes", "QuizHub");
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(q => q.Description)
            .IsRequired();
        builder.Property(q => q.TimeLimit)
            .IsRequired();
        builder.Property(q => q.Difficulty)
            .HasConversion<string>()
            .IsRequired();
        builder.Property(q => q.CreatedAt)
            .HasDefaultValueSql("NOW()");
        builder.Property(q => q.IsDeleted)
            .HasDefaultValue(false);
        builder.HasOne(q => q.Category)
            .WithMany()
            .HasForeignKey(q => q.CategoryId);
        builder.HasOne(q => q.Creator)
            .WithMany()
            .HasForeignKey(q => q.CreatedBy);
        builder.HasIndex(q => q.CategoryId);
        builder.HasIndex(q => q.Name);
    }
}
