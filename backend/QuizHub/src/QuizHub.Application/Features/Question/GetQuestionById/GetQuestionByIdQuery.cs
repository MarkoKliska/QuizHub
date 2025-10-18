using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Question.GetQuestion;

namespace QuizHub.Application.Features.Question.GetQuestionById;

public sealed record GetQuestionByIdQuery(Guid Id) 
    : IRequest<Result<GetQuestionByIdResponseDto>>;
