namespace QuizHub.Application.DTOs.Option.CreateOption;

public class CreateOptionResponseDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = default!;
    public bool IsCorrect { get; set; }
}
