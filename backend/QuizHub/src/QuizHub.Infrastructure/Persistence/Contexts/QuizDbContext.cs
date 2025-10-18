using Microsoft.EntityFrameworkCore;
using QuizHub.Domain.Entities;
using System.Reflection;

namespace QuizHub.Infrastructure.Persistence.Contexts;

public class QuizHubDbContext : DbContext
{
    public QuizHubDbContext(DbContextOptions<QuizHubDbContext> options) : base(options) { }
    public DbSet<User>? Users { get; set; }
    public DbSet<Quiz>? Quizzes { get; set; }
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Question>? Questions { get; set; }
    public DbSet<Option>? Options { get; set; }
    public DbSet<QuizAttempt>? QuizAttempts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("QuizHub");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
