using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Quiz.CreateQuiz;

namespace QuizHub.Application.Features.Quiz.CreateQuiz;

public sealed record CreateQuizCommand(CreateQuizRequestDto Request, Guid AdminId)
    : IRequest<Result<CreateQuizResponseDto>>;
