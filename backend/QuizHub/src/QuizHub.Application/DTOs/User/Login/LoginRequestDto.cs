namespace QuizHub.Application.DTOs.User.Login;

public class LoginRequestDto
{
    public string Identifier { get; set; } = default!;
    public string Password { get; set; } = default!;
}
