namespace QuizHub.Application.DTOs.Quiz.GetQuizById;

public class GetQuizByIdResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int TimeLimit { get; set; }
    public string Difficulty { get; set; } = default!;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}
