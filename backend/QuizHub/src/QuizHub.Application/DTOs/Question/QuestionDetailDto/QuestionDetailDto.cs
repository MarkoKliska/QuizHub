using QuizHub.Application.DTOs.QuizAttempt.QuizAttemptDetails;

namespace QuizHub.Application.DTOs.Question.QuestionDetailDto;

public class QuestionDetailDto
{
    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<OptionDetailDto> Options { get; set; } = [];
    public bool IsCorrect { get; set; }
    public string? UserAnswer { get; set; }
    public string? CorrectAnswer { get; set; }
}
