using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Category.DeleteCategory;

public sealed class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork uow
) : IRequestHandler<DeleteCategoryCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteCategoryCommand command, CancellationToken ct)
    {
        var category = await categoryRepository.GetByIdAsync(command.Id, ct);
        if (category == null || category.IsDeleted)
            return Result<Unit>.Failure("Category not found.");

        category.SetDeleted();
        await categoryRepository.UpdateAsync(category, ct);
        await uow.SaveChangesAsync(ct);

        return Result<Unit>.Success(Unit.Value);
    }
}