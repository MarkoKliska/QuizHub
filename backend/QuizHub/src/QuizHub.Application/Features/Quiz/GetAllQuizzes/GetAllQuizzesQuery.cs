using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Quiz.GetAllQuizzes;

namespace QuizHub.Application.Features.Quiz.GetAllQuizzes;

public sealed record GetAllQuizzesQuery 
    : IRequest<Result<IEnumerable<GetAllQuizzesResponseDto>>>;
