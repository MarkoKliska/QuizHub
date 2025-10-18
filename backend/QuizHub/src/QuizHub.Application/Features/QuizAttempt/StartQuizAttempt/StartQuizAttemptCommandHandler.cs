using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.QuizAttempt.StartQuizAttempt;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.QuizAttempt.StartQuizAttempt;

public sealed class StartQuizAttemptCommandHandler(
    IQuizRepository quizRepository,
    IQuizAttemptRepository quizAttemptRepository,
    IUnitOfWork uow
) : IRequestHandler<StartQuizAttemptCommand, Result<StartQuizAttemptResponseDto>>
{
    public async Task<Result<StartQuizAttemptResponseDto>> Handle(StartQuizAttemptCommand command, CancellationToken ct)
    {
        var req = command.Request;

        var quiz = await quizRepository.GetByIdAsync(req.QuizId, ct);
        if (quiz == null || quiz.IsDeleted)
            return Result<StartQuizAttemptResponseDto>.Failure("Quiz not found.");

        var existingAttempt = await quizAttemptRepository.GetByUserIdAsync(command.UserId, ct);
        if (existingAttempt.Any(a => a.QuizId == req.QuizId && !a.IsCompleted))
            return Result<StartQuizAttemptResponseDto>.Failure("User already has an active attempt for this quiz.");

        var attempt = new Domain.Entities.QuizAttempt(command.UserId, req.QuizId);

        await quizAttemptRepository.AddAsync(attempt, ct);
        await uow.SaveChangesAsync(ct);

        return Result<StartQuizAttemptResponseDto>.Success(new StartQuizAttemptResponseDto
        {
            Id = attempt.Id,
            UserId = attempt.UserId,
            QuizId = attempt.QuizId,
            QuizName = quiz.Name,
            StartTime = attempt.StartTime,
            EndTime = null,
            Score = 0, 
            Percentage = 0
        });
    }
}
