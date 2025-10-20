using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Quiz.GetFilteredQuizzes;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Quiz.GetFilteredQuizzes;

public sealed class GetFilteredQuizzesQueryHandler(IQuizRepository quizRepository)
    : IRequestHandler<GetFilteredQuizzesQuery, Result<IEnumerable<GetFilteredQuizzesResponseDto>>>
{
    public async Task<Result<IEnumerable<GetFilteredQuizzesResponseDto>>> Handle(GetFilteredQuizzesQuery query, CancellationToken ct)
    {
        var req = query.Request;

        var quizzesQuery = (await quizRepository.GetAllAsync(ct)).AsQueryable();

        if (req.CategoryId.HasValue)
        {
            quizzesQuery = quizzesQuery.Where(q => q.CategoryId == req.CategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(req.Difficulty))
        {
            if (!Enum.TryParse<Difficulty>(req.Difficulty, true, out var difficulty))
                return Result<IEnumerable<GetFilteredQuizzesResponseDto>>.Failure("Invalid difficulty level.");

            quizzesQuery = quizzesQuery.Where(q => q.Difficulty == difficulty);
        }

        if (!string.IsNullOrWhiteSpace(req.Search))
        {
            var searchLower = req.Search.ToLowerInvariant();
            quizzesQuery = quizzesQuery.Where(q => q.Name.ToLowerInvariant().Contains(searchLower) ||
                                                   q.Description.ToLowerInvariant().Contains(searchLower));
        }

        var quizzes = quizzesQuery.ToList();
        var response = quizzes.Select(q => new GetFilteredQuizzesResponseDto
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

        return Result<IEnumerable<GetFilteredQuizzesResponseDto>>.Success(response);
    }
}