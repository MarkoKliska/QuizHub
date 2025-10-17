namespace QuizHub.Application.DTOs.Category.GetCategoryById;

public class GetCategoryByIdResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
