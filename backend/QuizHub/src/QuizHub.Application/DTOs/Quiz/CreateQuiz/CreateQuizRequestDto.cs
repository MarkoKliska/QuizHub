namespace QuizHub.Application.DTOs.Quiz.CreateQuiz;

public class CreateQuizRequestDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int TimeLimit { get; set; }
    public string Difficulty { get; set; } = default!;
    public Guid CategoryId { get; set; }
}
