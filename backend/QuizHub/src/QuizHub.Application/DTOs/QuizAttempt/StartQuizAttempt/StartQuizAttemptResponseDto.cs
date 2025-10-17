namespace QuizHub.Application.DTOs.QuizAttempt.StartQuizAttempt;

public class StartQuizAttemptResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public string QuizName { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Score { get; set; }
    public double Percentage { get; set; }
}
