using Microsoft.EntityFrameworkCore;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;
using QuizHub.Infrastructure.Persistence.Contexts;

namespace QuizHub.Infrastructure.Persistence.Repositories;

public class UserRepository(
    QuizHubDbContext context
) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }
    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users
            .Where(u => !u.IsDeleted)
            .ToListAsync(cancellationToken);
    }
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
    }
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);
    }
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);
    }

}
