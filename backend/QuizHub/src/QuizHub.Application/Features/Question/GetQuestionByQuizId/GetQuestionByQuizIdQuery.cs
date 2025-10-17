using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Question.GetQuestionByQuizId;

namespace QuizHub.Application.Features.Question.GetQuestionByQuizId;

public sealed record GetQuestionsByQuizIdQuery(Guid QuizId) 
    : IRequest<Result<IEnumerable<GetQuestionByQuizIdResponseDto>>>;
