using QuizHub.Application.DTOs.QuizAttempt.UserAnswer;

namespace QuizHub.Application.DTOs.QuizAttempt.SubmitQuizAttempt;

public class SubmitQuizAttemptRequestDto
{
    public Guid QuizAttemptId { get; set; }
    public List<UserAnswerDto> Answers { get; set; } = new();
}
