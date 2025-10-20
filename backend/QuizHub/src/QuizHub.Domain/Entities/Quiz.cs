namespace QuizHub.Domain.Entities;
public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

public class Quiz
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public int TimeLimit { get; private set; }
    public Difficulty Difficulty { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; private set; }
    public User Creator { get; private set; } = null!;
    public bool IsDeleted { get; private set; }
    public List<Question> Questions { get; private set; } = new();

    private Quiz() { }

    public Quiz(string name, string description, int timeLimit, Difficulty difficulty, Guid categoryId, Guid createdBy)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        TimeLimit = timeLimit;
        Difficulty = difficulty;
        CategoryId = categoryId;
        CreatedBy = createdBy;
        IsDeleted = false;
    }

    public void Update(string name, string description, int timeLimit, Difficulty difficulty, Guid categoryId)
    {
        Name = name;
        Description = description;
        TimeLimit = timeLimit;
        Difficulty = difficulty;
        CategoryId = categoryId;
    }

    public void SetDeleted() => IsDeleted = true;
}
