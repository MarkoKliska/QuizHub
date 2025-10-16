namespace QuizHub.Application.DTOs.User.RegisterUser;

public class RegisterUserRequestDto
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? PictureBase64 { get; set; }
}
