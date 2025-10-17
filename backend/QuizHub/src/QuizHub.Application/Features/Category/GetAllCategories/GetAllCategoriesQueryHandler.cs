using MediatR;
using QuizHub.Application.DTOs.Category.GetAllCategories;
using QuizHub.Application.DTOs.Common;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Category.GetAllCategories;

public sealed class GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    : IRequestHandler<GetAllCategoriesQuery, Result<IEnumerable<GetAllCategoriesResponseDto>>>
{
    public async Task<Result<IEnumerable<GetAllCategoriesResponseDto>>> Handle(GetAllCategoriesQuery query, CancellationToken ct)
    {
        var categories = await categoryRepository.GetAllAsync(ct);

        var response = categories.Select(c => new GetAllCategoriesResponseDto
        {
            Id = c.Id,
            Name = c.Name
        });

        return Result<IEnumerable<GetAllCategoriesResponseDto>>.Success(response);
    }
}
