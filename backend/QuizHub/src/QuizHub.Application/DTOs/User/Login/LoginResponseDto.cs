namespace QuizHub.Application.DTOs.User.Login;

public class LoginResponseDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Role {  get; set; } = default!;
    public string Token {  get; set; } = default!;
}
