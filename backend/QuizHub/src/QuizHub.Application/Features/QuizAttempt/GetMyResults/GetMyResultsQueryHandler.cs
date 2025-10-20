using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.QuizAttempt.MyQuizAttempt;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.QuizAttempt.GetMyResults;

public sealed class GetMyResultsQueryHandler(
    IQuizAttemptRepository quizAttemptRepository
) : IRequestHandler<GetMyResultsQuery, Result<List<MyQuizAttemptDto>>>
{
    public async Task<Result<List<MyQuizAttemptDto>>> Handle(GetMyResultsQuery query, CancellationToken ct)
    {
        var attempts = await quizAttemptRepository.GetByUserIdAsync(query.UserId, ct);
        var completed = attempts.Where(a => a.IsCompleted).ToList();

        var result = completed.Select(a => new MyQuizAttemptDto
        {
            QuizAttemptId = a.Id,
            QuizId = a.QuizId,
            QuizName = a.Quiz.Name,
            CompletedAt = a.EndTime ?? DateTime.UtcNow,
            Score = a.Score ?? 0,
            Percentage = a.Percentage ?? 0,
            DurationSeconds = (int)((a.EndTime ?? DateTime.UtcNow) - a.StartTime).TotalSeconds
        }).ToList();

        return Result<List<MyQuizAttemptDto>>.Success(result);
    }
}
