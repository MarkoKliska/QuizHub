using Microsoft.EntityFrameworkCore;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;
using QuizHub.Infrastructure.Persistence.Contexts;

namespace QuizHub.Infrastructure.Persistence.Repositories;

public class OptionRepository(QuizHubDbContext context) : IOptionRepository
{
    public async Task AddAsync(Option option, CancellationToken ct)
    {
        await context.Options.AddAsync(option, ct);
    }

    public async Task<IEnumerable<Option>> GetByQuestionIdAsync(Guid questionId, CancellationToken ct)
    {
        return await context.Options
            .Where(o => o.QuestionId == questionId)
            .ToListAsync(ct);
    }

    public async Task UpdateAsync(Option option, CancellationToken ct)
    {
        context.Options.Update(option);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var option = await context.Options.FirstOrDefaultAsync(o => o.Id == id, ct);
        if (option != null)
        {
            context.Options.Remove(option);
        }
    }
}
