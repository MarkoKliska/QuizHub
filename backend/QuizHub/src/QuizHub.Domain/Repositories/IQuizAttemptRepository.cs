using QuizHub.Domain.Entities;

namespace QuizHub.Domain.Repositories;

public interface IQuizAttemptRepository
{
    Task AddAsync(QuizAttempt attempt, CancellationToken ct);
    Task<IEnumerable<QuizAttempt>> GetAllByQuizIdAsync(Guid quizId, CancellationToken ct);
    Task<IEnumerable<QuizAttempt>> GetAllAsync(CancellationToken ct);
    Task<IEnumerable<QuizAttempt>> GetByUserIdAsync(Guid userId, CancellationToken ct);
}