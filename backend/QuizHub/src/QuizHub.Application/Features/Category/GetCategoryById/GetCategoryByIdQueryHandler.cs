using MediatR;
using QuizHub.Application.DTOs.Category.GetCategoryById;
using QuizHub.Application.DTOs.Common;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Category.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoryByIdQuery, Result<GetCategoryByIdResponseDto>>
{
    public async Task<Result<GetCategoryByIdResponseDto>> Handle(GetCategoryByIdQuery query, CancellationToken ct)
    {
        var category = await categoryRepository.GetByIdAsync(query.Id, ct);
        if (category == null || category.IsDeleted)
            return Result<GetCategoryByIdResponseDto>.Failure("Category not found.");

        return Result<GetCategoryByIdResponseDto>.Success(new GetCategoryByIdResponseDto
        {
            Id = category.Id,
            Name = category.Name
        });
    }
}
