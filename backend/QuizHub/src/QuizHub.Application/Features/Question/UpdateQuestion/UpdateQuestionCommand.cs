using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Question.CreateQuestion;
using QuizHub.Application.DTOs.Question.UpdateQuestion;

namespace QuizHub.Application.Features.Question.UpdateQuestion;

public sealed record UpdateQuestionCommand(Guid Id, CreateQuestionRequestDto Request)
    : IRequest<Result<UpdateQuestionResponseDto>>;
