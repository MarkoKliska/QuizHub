namespace QuizHub.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsDeleted { get; private set; }

    private Category() { }

    public Category(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        IsDeleted = false;
    }

    public void Update(string name) => Name = name;
    public void SetDeleted() => IsDeleted = true;
}
