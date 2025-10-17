using Microsoft.EntityFrameworkCore;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;
using QuizHub.Infrastructure.Persistence.Contexts;

namespace QuizHub.Infrastructure.Persistence.Repositories;

public class QuestionRepository(QuizHubDbContext context) : IQuestionRepository
{
    public async Task AddAsync(Question question, CancellationToken ct)
    {
        await context.Questions.AddAsync(question, ct);
    }

    public async Task<Question?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted, ct);
    }

    public async Task<IEnumerable<Question>> GetByQuizIdAsync(Guid quizId, CancellationToken ct)
    {
        return await context.Questions
            .Include(q => q.Options)
            .Where(q => q.QuizId == quizId && !q.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task UpdateAsync(Question question, CancellationToken ct)
    {
        context.Questions.Update(question);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var question = await context.Questions.FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted, ct);
        if (question != null)
        {
            question.SetDeleted();
            context.Questions.Update(question);
        }
    }
}
