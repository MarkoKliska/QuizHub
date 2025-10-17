using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.QuizAttempt.GetAllQuizAttempt;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.QuizAttempt.GetQuizAttempt;

public sealed class GetAllQuizAttemptsQueryHandler(IQuizAttemptRepository quizAttemptRepository)
    : IRequestHandler<GetAllQuizAttemptsQuery, Result<IEnumerable<GetAllQuizAttemptsResponseDto>>>
{
    public async Task<Result<IEnumerable<GetAllQuizAttemptsResponseDto>>> Handle(GetAllQuizAttemptsQuery query, CancellationToken ct)
    {
        var attempts = await quizAttemptRepository.GetAllAsync(ct);
        var response = attempts.Select(a => new GetAllQuizAttemptsResponseDto
        {
            Id = a.Id,
            UserId = a.UserId,
            Username = a.User.UserName,
            QuizId = a.QuizId,
            QuizName = a.Quiz.Name,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Score = a.Score,
            Percentage = a.Percentage
        });

        return Result<IEnumerable<GetAllQuizAttemptsResponseDto>>.Success(response);
    }
}
