namespace QuizHub.Application.DTOs.QuizAttempt.GetQuizAttemptDetails;

public class GetQuizAttemptDetailsResponseDto
{
    public Guid AttemptId { get; set; }
    public Guid QuizId { get; set; }
    public string QuizName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int Score { get; set; }
    public double Percentage { get; set; }
    public double DurationSeconds { get; set; }

    public List<AnsweredQuestionDto> Questions { get; set; } = [];
}

public class AnsweredQuestionDto
{
    public Guid QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public List<OptionDto> Options { get; set; } = [];
    public List<Guid> SelectedOptionIds { get; set; } = [];
}

public class OptionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}
