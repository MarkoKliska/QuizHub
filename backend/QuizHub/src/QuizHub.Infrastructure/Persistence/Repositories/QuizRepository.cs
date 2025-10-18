using Microsoft.EntityFrameworkCore;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;
using QuizHub.Infrastructure.Persistence.Contexts;

namespace QuizHub.Infrastructure.Persistence.Repositories;

public class QuizRepository(QuizHubDbContext context) : IQuizRepository
{
    public async Task AddAsync(Quiz quiz, CancellationToken ct)
    {
        await context.Quizzes.AddAsync(quiz, ct);
    }

    public async Task<Quiz?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await context.Quizzes
            .Include(q => q.Category)
            .Include(q => q.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted, ct);
    }

    public async Task<IEnumerable<Quiz>> GetAllAsync(CancellationToken ct)
    {
        return await context.Quizzes
            .Include(q => q.Category)
            .Where(q => !q.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task UpdateAsync(Quiz quiz, CancellationToken ct)
    {
        context.Quizzes.Update(quiz);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var quiz = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted, ct);
        if (quiz != null)
        {
            quiz.SetDeleted();
            context.Quizzes.Update(quiz);
        }
    }
}