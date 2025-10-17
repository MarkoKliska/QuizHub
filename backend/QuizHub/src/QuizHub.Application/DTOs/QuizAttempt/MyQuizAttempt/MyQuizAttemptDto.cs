namespace QuizHub.Application.DTOs.QuizAttempt.MyQuizAttempt;

public class MyQuizAttemptDto
{
    public Guid QuizAttemptId { get; set; }
    public Guid QuizId { get; set; }
    public string QuizName { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
    public int Score { get; set; }
    public double Percentage { get; set; }
    public int DurationSeconds { get; set; }
}
