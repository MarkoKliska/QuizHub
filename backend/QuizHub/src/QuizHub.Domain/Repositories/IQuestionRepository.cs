using QuizHub.Domain.Entities;

namespace QuizHub.Domain.Repositories;


public interface IQuestionRepository
{
    Task AddAsync(Question question, CancellationToken ct);
    Task<Question?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<Question>> GetByQuizIdAsync(Guid quizId, CancellationToken ct);
    Task UpdateAsync(Question question, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}