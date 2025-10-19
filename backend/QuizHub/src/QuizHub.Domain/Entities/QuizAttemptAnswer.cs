namespace QuizHub.Domain.Entities;

public class QuizAttemptAnswer
{
    public Guid Id { get; private set; }
    public Guid QuizAttemptId { get; private set; }
    public Guid QuestionId { get; private set; }
    public List<Guid>? SelectedOptionIds { get; private set; }
    public string? TextAnswer { get; private set; }
    public bool IsCorrect { get; private set; }
    public int PointsAwarded { get; private set; }
    public QuizAttempt QuizAttempt { get; private set; } = null!;
    public Question Question { get; private set; } = null!;

    private QuizAttemptAnswer() { }

    public QuizAttemptAnswer(
        Guid quizAttemptId,
        Guid questionId,
        List<Guid>? selectedOptionIds,
        string? textAnswer,
        bool isCorrect,
        int pointsAwarded)
    {
        Id = Guid.NewGuid();
        QuizAttemptId = quizAttemptId;
        QuestionId = questionId;
        SelectedOptionIds = selectedOptionIds;
        TextAnswer = textAnswer;
        IsCorrect = isCorrect;
        PointsAwarded = pointsAwarded;
    }
}
