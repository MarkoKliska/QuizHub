using QuizHub.Domain.Entities;

namespace QuizHub.Domain.Repositories;

public interface IQuizRepository
{
    Task AddAsync(Quiz quiz, CancellationToken ct);
    Task<Quiz?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<Quiz>> GetAllAsync(CancellationToken ct);
    Task UpdateAsync(Quiz quiz, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}