using QuizHub.Application.DTOs.Option.CreateOption;

namespace QuizHub.Application.DTOs.Question.GetQuestion;

public class GetQuestionByIdResponseDto
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public string Text { get; set; } = default!;
    public string Type { get; set; } = default!;
    public int Points { get; set; }
    public string? CorrectAnswer { get; set; }
    public List<CreateOptionResponseDto> Options { get; set; } = new();
}
