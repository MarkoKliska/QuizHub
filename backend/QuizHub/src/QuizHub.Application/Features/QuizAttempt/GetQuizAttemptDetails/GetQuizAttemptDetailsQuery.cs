using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.QuizAttempt.QuizAttemptDetails;

namespace QuizHub.Application.Features.QuizAttempt.GetQuizAttemptDetails;


public sealed record GetQuizAttemptDetailsQuery(Guid QuizAttemptId)
    : IRequest<Result<QuizAttemptDetailsDto>>;
