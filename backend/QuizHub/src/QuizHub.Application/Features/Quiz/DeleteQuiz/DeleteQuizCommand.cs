using MediatR;
using QuizHub.Application.DTOs.Common;

namespace QuizHub.Application.Features.Quiz.DeleteQuiz;

public sealed record DeleteQuizCommand(Guid Id) 
    : IRequest<Result<Unit>>;
