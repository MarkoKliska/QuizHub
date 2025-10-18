using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.QuizAttempt.StartQuizAttempt;

namespace QuizHub.Application.Features.QuizAttempt.StartQuizAttempt;

public sealed record StartQuizAttemptCommand(StartQuizAttemptRequestDto Request, Guid UserId)
    : IRequest<Result<StartQuizAttemptResponseDto>>;
