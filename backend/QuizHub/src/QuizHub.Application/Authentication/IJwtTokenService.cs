using QuizHub.Domain.Entities;

namespace QuizHub.Application.Authentication;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string email, UserRole role);
}
