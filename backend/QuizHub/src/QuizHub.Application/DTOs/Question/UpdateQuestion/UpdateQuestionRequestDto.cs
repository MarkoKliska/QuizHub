using QuizHub.Application.DTOs.Option.CreateOption;

namespace QuizHub.Application.DTOs.Question.UpdateQuestion;

public class UpdateQuestionRequestDto
{
    public Guid QuizId { get; set; }
    public string Text { get; set; } = default!;
    public string Type { get; set; } = default!;
    public int Points { get; set; }
    public string? CorrectAnswer { get; set; }
    public List<CreateOptionRequestDto> Options { get; set; } = new();
}
