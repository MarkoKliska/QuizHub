using MediatR;
using QuizHub.Application.DTOs.Category.CreateCategory;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Category.CreateCategory;

public sealed class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork uow
) : IRequestHandler<CreateCategoryCommand, Result<CreateCategoryResponseDto>>
{
    public async Task<Result<CreateCategoryResponseDto>> Handle(CreateCategoryCommand command, CancellationToken ct)
    {
        var req = command.Request;

        if (string.IsNullOrWhiteSpace(req.Name) || req.Name.Length > 50)
            return Result<CreateCategoryResponseDto>.Failure("Category name is required and must be 50 characters or less.");

        var existingCategory = await categoryRepository.GetByNameAsync(req.Name, ct);
        if (existingCategory != null && !existingCategory.IsDeleted)
            return Result<CreateCategoryResponseDto>.Failure("Category with this name already exists.");

        var category = new Domain.Entities.Category(req.Name);

        await categoryRepository.AddAsync(category, ct);
        await uow.SaveChangesAsync(ct);

        return Result<CreateCategoryResponseDto>.Success(new CreateCategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        });
    }
}
