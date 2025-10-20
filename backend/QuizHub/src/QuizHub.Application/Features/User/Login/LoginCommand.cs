using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.User.Login;

namespace QuizHub.Application.Features.User.Login;

public sealed record LoginCommand(LoginRequestDto Request)
    : IRequest<Result<LoginResponseDto>>;
