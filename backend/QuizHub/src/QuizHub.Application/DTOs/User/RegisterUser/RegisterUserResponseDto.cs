namespace QuizHub.Application.DTOs.User.RegisterUser;

public class RegisterUserResponseDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
}
