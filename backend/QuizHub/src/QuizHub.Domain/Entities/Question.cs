namespace QuizHub.Domain.Entities;
public enum QuestionType
{
    SingleChoice,
    MultipleChoice,
    TrueFalse,
    FillInBlank
}

public class Question
{
    public Guid Id { get; private set; }
    public Guid QuizId { get; private set; }
    public Quiz Quiz { get; private set; } = null!;
    public string Text { get; private set; } = null!;
    public QuestionType Type { get; private set; }
    public int Points { get; private set; }
    public string? CorrectAnswer { get; private set; }
    public bool IsDeleted { get; private set; }
    public List<Option> Options { get; private set; } = new();

    private Question() { }

    public Question(Guid quizId, string text, QuestionType type, int points, string? correctAnswer = null)
    {
        Id = Guid.NewGuid();
        QuizId = quizId;
        Text = text;
        Type = type;
        Points = points;
        CorrectAnswer = correctAnswer;
        IsDeleted = false;
    }

    public void Update(string text, int points, string? correctAnswer = null)
    {
        Text = text;
        Points = points;
        CorrectAnswer = correctAnswer;
    }
    public void UpdateType(QuestionType type)
    {
        Type = type;
    }
    public void SetDeleted() => IsDeleted = true;
}
