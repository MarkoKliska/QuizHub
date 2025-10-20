namespace QuizHub.Application.DTOs.QuizAttempt.UserAnswer;

public class UserAnswerDto
{
    public Guid QuestionId { get; set; }
    public List<Guid>? SelectedOptionIds { get; set; }
    public string? TextAnswer { get; set; }
}
