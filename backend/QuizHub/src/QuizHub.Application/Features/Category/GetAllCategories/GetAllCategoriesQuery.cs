using MediatR;
using QuizHub.Application.DTOs.Category.GetAllCategories;
using QuizHub.Application.DTOs.Common;

namespace QuizHub.Application.Features.Category.GetAllCategories;

public sealed record GetAllCategoriesQuery 
    : IRequest<Result<IEnumerable<GetAllCategoriesResponseDto>>>;
