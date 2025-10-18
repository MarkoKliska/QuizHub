namespace QuizHub.Application.DTOs.QuizAttempt.GetAllQuizAttempt;

public class GetAllQuizAttemptsResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = default!;
    public Guid QuizId { get; set; }
    public string QuizName { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int ?Score { get; set; }
    public double ?Percentage { get; set; }
}
