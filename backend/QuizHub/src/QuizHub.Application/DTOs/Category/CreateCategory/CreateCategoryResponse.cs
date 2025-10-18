namespace QuizHub.Application.DTOs.Category.CreateCategory;

public class CreateCategoryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
