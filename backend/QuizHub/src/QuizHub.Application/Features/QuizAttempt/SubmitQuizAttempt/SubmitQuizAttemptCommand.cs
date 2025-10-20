using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.QuizAttempt.SubmitQuizAttempt;

namespace QuizHub.Application.Features.QuizAttempt.SubmitQuizAttempt;

public sealed record SubmitQuizAttemptCommand(SubmitQuizAttemptRequestDto Request, Guid UserId)
    : IRequest<Result<SubmitQuizAttemptResponseDto>>;
