using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Quiz.CreateQuiz;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Quiz.CreateQuiz;

public sealed class CreateQuizCommandHandler(
    IQuizRepository quizRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork uow
) : IRequestHandler<CreateQuizCommand, Result<CreateQuizResponseDto>>
{
    public async Task<Result<CreateQuizResponseDto>> Handle(CreateQuizCommand command, CancellationToken ct)
    {
        var req = command.Request;

        if (string.IsNullOrWhiteSpace(req.Name) || req.Name.Length > 100)
            return Result<CreateQuizResponseDto>.Failure("Quiz name is required and must be 100 characters or less.");

        if (req.TimeLimit <= 0)
            return Result<CreateQuizResponseDto>.Failure("Time limit must be greater than 0.");

        if (!Enum.TryParse<Difficulty>(req.Difficulty, true, out var difficulty))
            return Result<CreateQuizResponseDto>.Failure("Invalid difficulty level.");

        var category = await categoryRepository.GetByIdAsync(req.CategoryId, ct);
        if (category == null || category.IsDeleted)
            return Result<CreateQuizResponseDto>.Failure("Category not found.");

        var quiz = new Domain.Entities.Quiz(
            name: req.Name,
            description: req.Description,
            timeLimit: req.TimeLimit,
            difficulty: difficulty,
            categoryId: req.CategoryId,
            createdBy: command.AdminId
        );

        await quizRepository.AddAsync(quiz, ct);
        await uow.SaveChangesAsync(ct);

        return Result<CreateQuizResponseDto>.Success(new CreateQuizResponseDto
        {
            Id = quiz.Id,
            Name = quiz.Name,
            Description = quiz.Description,
            TimeLimit = quiz.TimeLimit,
            Difficulty = quiz.Difficulty.ToString(),
            CategoryId = quiz.CategoryId,
            CategoryName = category.Name,
            CreatedBy = quiz.CreatedBy,
            CreatedAt = quiz.CreatedAt
        });
    }
}
