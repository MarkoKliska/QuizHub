using Microsoft.EntityFrameworkCore;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;
using QuizHub.Infrastructure.Persistence.Contexts;

namespace QuizHub.Infrastructure.Persistence.Repositories;

public class QuizAttemptAnswerRepository(QuizHubDbContext context) : IQuizAttemptAnswerRepository
{

    public async Task<IEnumerable<QuizAttemptAnswer>> GetByQuizAttemptIdAsync(Guid quizAttemptId, CancellationToken ct)
    {
        return await context.QuizAttemptAnswers
            .Include(a => a.Question)
                .ThenInclude(q => q.Options)
            .Where(a => a.QuizAttemptId == quizAttemptId)
            .ToListAsync(ct);
    }

    public async Task AddRangeAsync(IEnumerable<QuizAttemptAnswer> answers, CancellationToken ct)
    {
        await context.QuizAttemptAnswers.AddRangeAsync(answers, ct);
    }
}
