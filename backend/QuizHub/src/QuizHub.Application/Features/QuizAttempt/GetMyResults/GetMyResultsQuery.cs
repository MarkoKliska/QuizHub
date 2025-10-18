using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.QuizAttempt.MyQuizAttempt;

namespace QuizHub.Application.Features.QuizAttempt.GetMyResults;

public sealed record GetMyResultsQuery(Guid UserId)
    : IRequest<Result<List<MyQuizAttemptDto>>>;
