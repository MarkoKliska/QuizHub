using QuizHub.Domain.Entities;

namespace QuizHub.Domain.Repositories;

public interface IQuizAttemptAnswerRepository
{
    Task<IEnumerable<QuizAttemptAnswer>> GetByQuizAttemptIdAsync(Guid quizAttemptId, CancellationToken ct);
    Task AddRangeAsync(IEnumerable<QuizAttemptAnswer> answers, CancellationToken ct);
}
