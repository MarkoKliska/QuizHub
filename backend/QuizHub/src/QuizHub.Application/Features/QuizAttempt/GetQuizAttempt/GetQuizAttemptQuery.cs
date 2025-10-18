using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.QuizAttempt.GetAllQuizAttempt;

namespace QuizHub.Application.Features.QuizAttempt.GetQuizAttempt;

public sealed record GetAllQuizAttemptsQuery 
    : IRequest<Result<IEnumerable<GetAllQuizAttemptsResponseDto>>>;

