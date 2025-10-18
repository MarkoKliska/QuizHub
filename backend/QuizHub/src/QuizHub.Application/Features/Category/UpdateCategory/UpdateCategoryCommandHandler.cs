using MediatR;
using QuizHub.Application.DTOs.Category.UpdateCategory;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Category.UpdateCategory;

public sealed class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork uow
) : IRequestHandler<UpdateCategoryCommand, Result<UpdateCategoryResponseDto>>
{
    public async Task<Result<UpdateCategoryResponseDto>> Handle(UpdateCategoryCommand command, CancellationToken ct)
    {
        var req = command.Request;

        if (string.IsNullOrWhiteSpace(req.Name) || req.Name.Length > 50)
            return Result<UpdateCategoryResponseDto>.Failure("Category name is required and must be 50 characters or less.");

        var existingCategory = await categoryRepository.GetByNameAsync(req.Name, ct);
        if (existingCategory != null && existingCategory.Id != command.Id && !existingCategory.IsDeleted)
            return Result<UpdateCategoryResponseDto>.Failure("Category with this name already exists.");

        var category = await categoryRepository.GetByIdAsync(command.Id, ct);
        if (category == null || category.IsDeleted)
            return Result<UpdateCategoryResponseDto>.Failure("Category not found.");

        category.Update(req.Name);

        await categoryRepository.UpdateAsync(category, ct);
        await uow.SaveChangesAsync(ct);

        return Result<UpdateCategoryResponseDto>.Success(new UpdateCategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name
        });
    }
}
