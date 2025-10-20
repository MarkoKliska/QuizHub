namespace QuizHub.Domain.Entities;

public class Option
{
    public Guid Id { get; private set; }
    public Guid QuestionId { get; private set; }
    public Question Question { get; private set; } = null!;
    public string Text { get; private set; } = null!;
    public bool IsCorrect { get; private set; }

    private Option() { }

    public Option(Guid questionId, string text, bool isCorrect)
    {
        Id = Guid.NewGuid();
        QuestionId = questionId;
        Text = text;
        IsCorrect = isCorrect;
    }

    public void Update(string text, bool isCorrect)
    {
        Text = text;
        IsCorrect = isCorrect;
    }
}
