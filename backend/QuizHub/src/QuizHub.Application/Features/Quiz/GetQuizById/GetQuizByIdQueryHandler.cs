using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Quiz.GetQuizById;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Quiz.GetQuizById;

public sealed class GetQuizByIdQueryHandler(IQuizRepository quizRepository)
    : IRequestHandler<GetQuizByIdQuery, Result<GetQuizByIdResponseDto>>
{
    public async Task<Result<GetQuizByIdResponseDto>> Handle(GetQuizByIdQuery query, CancellationToken ct)
    {
        var quiz = await quizRepository.GetByIdAsync(query.Id, ct);
        if (quiz == null || quiz.IsDeleted)
            return Result<GetQuizByIdResponseDto>.Failure("Quiz not found.");

        return Result<GetQuizByIdResponseDto>.Success(new GetQuizByIdResponseDto
        {
            Id = quiz.Id,
            Name = quiz.Name,
            Description = quiz.Description,
            TimeLimit = quiz.TimeLimit,
            Difficulty = quiz.Difficulty.ToString(),
            CategoryId = quiz.CategoryId,
            CategoryName = quiz.Category.Name,
            CreatedBy = quiz.CreatedBy,
            CreatedAt = quiz.CreatedAt
        });
    }
}
