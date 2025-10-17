namespace QuizHub.Application.DTOs.QuizAttempt.GetUserQuizAttempts;

public class GetUserQuizAttemptsResponseDto
{
    public Guid AttemptId { get; set; }
    public Guid QuizId { get; set; }
    public string QuizName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Score { get; set; }
    public double Percentage { get; set; }
}
