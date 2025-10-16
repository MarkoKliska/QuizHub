using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.User.RegisterUser;

namespace QuizHub.Application.Features.User.RegisterUser;

public sealed record RegisterUserCommand(RegisterUserRequestDto Request)
    : IRequest<Result<RegisterUserResponseDto>>;
