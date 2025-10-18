using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Quiz.GetAllQuizzes;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Quiz.GetAllQuizzes;

public sealed class GetAllQuizzesQueryHandler(IQuizRepository quizRepository)
    : IRequestHandler<GetAllQuizzesQuery, Result<IEnumerable<GetAllQuizzesResponseDto>>>
{
    public async Task<Result<IEnumerable<GetAllQuizzesResponseDto>>> Handle(GetAllQuizzesQuery query, CancellationToken ct)
    {
        var quizzes = await quizRepository.GetAllAsync(ct);
        var response = quizzes.Select(q => new GetAllQuizzesResponseDto
        {
            Id = q.Id,
            Name = q.Name,
            Description = q.Description,
            TimeLimit = q.TimeLimit,
            Difficulty = q.Difficulty.ToString(),
            CategoryId = q.CategoryId,
            CategoryName = q.Category.Name,
            CreatedBy = q.CreatedBy,
            CreatedAt = q.CreatedAt
        });

        return Result<IEnumerable<GetAllQuizzesResponseDto>>.Success(response);
    }
}
