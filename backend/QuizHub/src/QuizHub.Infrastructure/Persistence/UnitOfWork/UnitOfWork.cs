using Microsoft.EntityFrameworkCore;
using QuizHub.Application.Interfaces;
using QuizHub.Infrastructure.Persistence.Contexts;

namespace QuizHub.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork(
    QuizHubDbContext context
) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}
