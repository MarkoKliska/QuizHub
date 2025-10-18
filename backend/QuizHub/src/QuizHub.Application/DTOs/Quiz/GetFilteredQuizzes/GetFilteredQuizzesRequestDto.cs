namespace QuizHub.Application.DTOs.Quiz.GetFilteredQuizzes;

public class GetFilteredQuizzesRequestDto
{
    public Guid? CategoryId { get; set; }
    public string? Difficulty { get; set; }
    public string? Search { get; set; }
}
