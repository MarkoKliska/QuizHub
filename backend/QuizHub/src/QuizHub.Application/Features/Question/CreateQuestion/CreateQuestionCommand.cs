using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Question.CreateQuestion;

namespace QuizHub.Application.Features.Question.CreateQuestion;

public sealed record CreateQuestionCommand(CreateQuestionRequestDto Request)
    : IRequest<Result<CreateQuestionResponseDto>>;
