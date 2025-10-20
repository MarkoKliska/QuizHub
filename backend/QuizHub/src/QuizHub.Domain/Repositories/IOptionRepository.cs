using QuizHub.Domain.Entities;

namespace QuizHub.Domain.Repositories;

public interface IOptionRepository
{
    Task AddAsync(Option option, CancellationToken ct);
    Task<IEnumerable<Option>> GetByQuestionIdAsync(Guid questionId, CancellationToken ct);
    Task UpdateAsync(Option option, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}