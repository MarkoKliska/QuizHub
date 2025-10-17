using Microsoft.EntityFrameworkCore;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;
using QuizHub.Infrastructure.Persistence.Contexts;

namespace QuizHub.Infrastructure.Persistence.Repositories;


public class QuizAttemptRepository(QuizHubDbContext context) : IQuizAttemptRepository
{
    public async Task AddAsync(QuizAttempt attempt, CancellationToken ct)
    {
        await context.QuizAttempts.AddAsync(attempt, ct);
    }

    public async Task<IEnumerable<QuizAttempt>> GetAllByQuizIdAsync(Guid quizId, CancellationToken ct)
    {
        return await context.QuizAttempts
            .Include(a => a.User)
            .Include(a => a.Quiz)
            .Where(a => a.QuizId == quizId && a.IsCompleted)
            .OrderByDescending(a => a.Score)
            .ThenBy(a => a.EndTime)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<QuizAttempt>> GetAllAsync(CancellationToken ct)
    {
        return await context.QuizAttempts
            .Include(a => a.User)
            .Include(a => a.Quiz)
            .Where(a => a.IsCompleted)
            .OrderByDescending(a => a.Score)
            .ThenBy(a => a.EndTime)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<QuizAttempt>> GetByUserIdAsync(Guid userId, CancellationToken ct)
    {
        return await context.QuizAttempts
            .Include(a => a.Quiz)
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.EndTime)
            .ToListAsync(ct);
    }
}
