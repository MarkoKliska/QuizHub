using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Quiz.CreateQuiz;
using QuizHub.Application.DTOs.Quiz.UpdateQuiz;

namespace QuizHub.Application.Features.Quiz.UpdateQuiz;

public sealed record UpdateQuizCommand(Guid Id, CreateQuizRequestDto Request)
    : IRequest<Result<UpdateQuizResponseDto>>;
