using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Quiz.GetFilteredQuizzes;

namespace QuizHub.Application.Features.Quiz.GetFilteredQuizzes;

public sealed record GetFilteredQuizzesQuery(GetFilteredQuizzesRequestDto Request)
    : IRequest<Result<IEnumerable<GetFilteredQuizzesResponseDto>>>;
