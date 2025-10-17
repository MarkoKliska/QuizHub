using QuizHub.Domain.Entities;

namespace QuizHub.Domain.Repositories;

public interface ICategoryRepository
{
    Task AddAsync(Category category, CancellationToken ct);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Category?> GetByNameAsync(string name, CancellationToken ct);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct);
    Task UpdateAsync(Category category, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}