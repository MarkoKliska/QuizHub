using MediatR;
using QuizHub.Application.Authentication;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.User.Login;
using QuizHub.Application.Utils;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.User.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtTokenService jwtService
) : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    public async Task<Result<LoginResponseDto>> Handle(LoginCommand command, CancellationToken ct)
    {
        var req = command.Request;

        var user = await userRepository.GetByUsernameAsync(req.Identifier, ct)
                   ?? await userRepository.GetByEmailAsync(req.Identifier, ct);

        if (user is null)
            return Result<LoginResponseDto>.Failure("User not found.");

        if (!PasswordHasher.VerifyPassword(req.Password, user.PasswordHash))
            return Result<LoginResponseDto>.Failure("Invalid credentials.");

        var token = jwtService.GenerateToken(user.Id, user.Email, user.Role);

        var response = new LoginResponseDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = token
        };

        return Result<LoginResponseDto>.Success(response);
    }
}
