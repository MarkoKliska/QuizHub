namespace QuizHub.Domain.Entities;

public class QuizAttempt
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid QuizId { get; private set; }
    public Quiz Quiz { get; private set; } = null!;
    public DateTime StartTime { get; private set; } = DateTime.UtcNow;
    public DateTime? EndTime { get; private set; }
    public int? Score { get; private set; }
    public double? Percentage { get; private set; }
    public bool IsCompleted { get; private set; }

    private QuizAttempt() { }

    public QuizAttempt(Guid userId, Guid quizId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        QuizId = quizId;
        IsCompleted = false;
    }

    public void Complete(int score, double percentage)
    {
        Score = score;
        Percentage = percentage;
        EndTime = DateTime.UtcNow;
        IsCompleted = true;
    }

    public void SetIsCompleted()
    {
        IsCompleted = true;
    }

    public void SetEndTime()
    {
        EndTime = DateTime.UtcNow;
    }
}
