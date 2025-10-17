using MediatR;
using QuizHub.Application.DTOs.Category.CreateCategory;
using QuizHub.Application.DTOs.Common;
namespace QuizHub.Application.Features.Category.CreateCategory;

public sealed record CreateCategoryCommand(CreateCategoryRequestDto Request)
    : IRequest<Result<CreateCategoryResponseDto>>;
