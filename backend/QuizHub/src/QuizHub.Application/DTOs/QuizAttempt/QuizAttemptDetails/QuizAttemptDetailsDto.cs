namespace QuizHub.Application.DTOs.QuizAttempt.QuizAttemptDetails;

public class QuizAttemptDetailsDto
{
    public string QuizName { get; set; } = string.Empty;
    public int Score { get; set; }
    public double Percentage { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public List<QuestionDetailDto> Questions { get; set; } = [];
}

public class QuestionDetailDto
{
    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<OptionDetailDto> Options { get; set; } = [];
    public bool IsCorrect { get; set; }
}

public class OptionDetailDto
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public bool Selected { get; set; }
}
