using Microsoft.EntityFrameworkCore;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;
using QuizHub.Infrastructure.Persistence.Contexts;

namespace QuizHub.Infrastructure.Persistence.Repositories;

public class CategoryRepository(QuizHubDbContext context) : ICategoryRepository
{
    public async Task AddAsync(Category category, CancellationToken ct)
    {
        await context.Categories.AddAsync(category, ct);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await context.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);
    }

    public async Task<Category?> GetByNameAsync(string name, CancellationToken ct)
    {
        return await context.Categories.FirstOrDefaultAsync(c => c.Name == name && !c.IsDeleted, ct);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct)
    {
        return await context.Categories
            .Where(c => !c.IsDeleted)
            .ToListAsync(ct);
    }

    public async Task UpdateAsync(Category category, CancellationToken ct)
    {
        context.Categories.Update(category);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);
        if (category != null)
        {
            category.SetDeleted();
            context.Categories.Update(category);
        }
    }
}
