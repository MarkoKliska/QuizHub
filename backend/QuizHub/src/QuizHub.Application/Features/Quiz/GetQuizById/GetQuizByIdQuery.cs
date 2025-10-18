using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Quiz.GetQuizById;

namespace QuizHub.Application.Features.Quiz.GetQuizById;

public sealed record GetQuizByIdQuery(Guid Id) 
    : IRequest<Result<GetQuizByIdResponseDto>>;
