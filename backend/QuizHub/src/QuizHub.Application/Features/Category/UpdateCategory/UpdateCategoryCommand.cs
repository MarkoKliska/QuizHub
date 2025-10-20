using MediatR;
using QuizHub.Application.DTOs.Category.CreateCategory;
using QuizHub.Application.DTOs.Category.UpdateCategory;
using QuizHub.Application.DTOs.Common;

namespace QuizHub.Application.Features.Category.UpdateCategory;

public sealed record UpdateCategoryCommand(Guid Id, CreateCategoryRequestDto Request)
    : IRequest<Result<UpdateCategoryResponseDto>>;
