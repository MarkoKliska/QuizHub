using MediatR;
using QuizHub.Application.DTOs.Category.GetCategoryById;
using QuizHub.Application.DTOs.Common;

namespace QuizHub.Application.Features.Category.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid Id) 
    : IRequest<Result<GetCategoryByIdResponseDto>>;
