using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Quiz.UpdateQuiz;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Quiz.UpdateQuiz;

public sealed class UpdateQuizCommandHandler(
    IQuizRepository quizRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork uow
) : IRequestHandler<UpdateQuizCommand, Result<UpdateQuizResponseDto>>
{
    public async Task<Result<UpdateQuizResponseDto>> Handle(UpdateQuizCommand command, CancellationToken ct)
    {
        var req = command.Request;

        if (string.IsNullOrWhiteSpace(req.Name) || req.Name.Length > 100)
            return Result<UpdateQuizResponseDto>.Failure("Quiz name is required and must be 100 characters or less.");

        if (req.TimeLimit <= 0)
            return Result<UpdateQuizResponseDto>.Failure("Time limit must be greater than 0.");

        if (!Enum.TryParse<Difficulty>(req.Difficulty, true, out var difficulty))
            return Result<UpdateQuizResponseDto>.Failure("Invalid difficulty level.");

        var category = await categoryRepository.GetByIdAsync(req.CategoryId, ct);
        if (category == null || category.IsDeleted)
            return Result<UpdateQuizResponseDto>.Failure("Category not found.");

        var quiz = await quizRepository.GetByIdAsync(command.Id, ct);
        if (quiz == null || quiz.IsDeleted)
            return Result<UpdateQuizResponseDto>.Failure("Quiz not found.");

        quiz.Update(req.Name, req.Description, req.TimeLimit, difficulty, req.CategoryId);

        await quizRepository.UpdateAsync(quiz, ct);
        await uow.SaveChangesAsync(ct);

        return Result<UpdateQuizResponseDto>.Success(new UpdateQuizResponseDto
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
