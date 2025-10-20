namespace QuizHub.Domain.Entities;
public enum UserRole
{
    User = 0,
    Admin = 1
}
public class User
{
    public Guid Id { get; private set; }
    public string UserName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public byte[]? ProfileImage { get; private set; }      
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public UserRole Role { get; private set; } = UserRole.User;
    public bool IsDeleted { get; private set; }
    public User() { }
    public User(string userName, string email, string passwordHash, byte[]? profileImage)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        ProfileImage = profileImage;
        CreatedAt = DateTime.UtcNow;
        Role = UserRole.User;
        IsDeleted = false;
    }
    public void SetRole(UserRole role)
    {
        Role = role;
    }
    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
    public void SetUserName(string userName)
    {
        UserName = userName;
    }
    public void SetEmail(string email)
    {
        Email = email;
    }
    public void SetDeleted()
    {
        IsDeleted = true;
    }
    public void SetProfileImage(byte[] profileImage)
    {
        ProfileImage = profileImage;
    }
}
