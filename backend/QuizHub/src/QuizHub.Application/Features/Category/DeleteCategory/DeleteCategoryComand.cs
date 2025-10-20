using MediatR;
using QuizHub.Application.DTOs.Common;

namespace QuizHub.Application.Features.Category.DeleteCategory;

public sealed record DeleteCategoryCommand(Guid Id) 
    : IRequest<Result<Unit>>;
