using MediatR;
using QuizHub.Application.Authentication;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.User.RegisterUser;
using QuizHub.Application.Interfaces;
using QuizHub.Application.Utils;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;
using System.Text.RegularExpressions;

namespace QuizHub.Application.Features.User.RegisterUser;

public sealed class RegisterUserCommandHandler (
    IUserRepository users,
    IUnitOfWork uow,
    IJwtTokenService jwtService
) : IRequestHandler<RegisterUserCommand, Result<RegisterUserResponseDto>>
{
    public async Task<Result<RegisterUserResponseDto>> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        var req = command.Request;

        if (string.IsNullOrWhiteSpace(req.Password) || req.Password.Length < 8)
            return Result<RegisterUserResponseDto>.Failure("Password must be at least 8 characters long.");

        if (!Regex.IsMatch(req.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return Result<RegisterUserResponseDto>.Failure("Invalid email format.");

        var existingUser = await users.GetByUsernameAsync(req.Username, ct);
        if (existingUser is not null)
            return Result<RegisterUserResponseDto>.Failure("Username already exists.");

        var existingUserEmail = await users.GetByEmailAsync(req.Email, ct);
        if (existingUserEmail is not null)
            return Result<RegisterUserResponseDto>.Failure("Email is already in use.");

        var passwordHash = PasswordHasher.HashPassword(req.Password);

        byte[]? imageBytes = null;
        if (!string.IsNullOrEmpty(req.PictureBase64))
        {
            try
            {
                imageBytes = Convert.FromBase64String(req.PictureBase64);
            }
            catch
            {
                return Result<RegisterUserResponseDto>.Failure("Invalid picture format (must be Base64).");
            }
        }

        var user = new QuizHub.Domain.Entities.User(
            userName: req.Username,
            email: req.Email,
            passwordHash: passwordHash,
            profileImage: imageBytes
        );

        await users.AddAsync(user, ct);
        await uow.SaveChangesAsync(ct);

        var token = jwtService.GenerateToken(user.Id, user.Email, user.Role);

        var response = new RegisterUserResponseDto
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Token = token
        };

        return Result<RegisterUserResponseDto>.Success(response);
    }
}
