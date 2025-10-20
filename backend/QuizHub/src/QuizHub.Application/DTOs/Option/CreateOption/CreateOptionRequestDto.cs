namespace QuizHub.Application.DTOs.Option.CreateOption;

public class CreateOptionRequestDto
{
    public string Text { get; set; } = default!;
    public bool IsCorrect { get; set; }
}
