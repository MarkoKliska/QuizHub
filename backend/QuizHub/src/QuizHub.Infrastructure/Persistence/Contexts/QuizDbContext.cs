using Microsoft.EntityFrameworkCore;
using QuizHub.Domain.Entities;
using System.Reflection;

namespace QuizHub.Infrastructure.Persistence.Contexts;

public class QuizHubDbContext : DbContext
{
    public QuizHubDbContext(DbContextOptions<QuizHubDbContext> options) : base(options) { }
    public DbSet<User>? Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("QuizHub");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
