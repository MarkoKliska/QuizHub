using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizHub.Application.Authentication;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Repositories;
using QuizHub.Infrastructure.Persistence.Contexts;
using QuizHub.Infrastructure.Persistence.Repositories;
using QuizHub.Infrastructure.Persistence.UnitOfWork;
using QuizHub.Infrastructure.Services;

namespace QuizHub.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddDbContext<QuizHubDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), sql =>
                sql.MigrationsAssembly(typeof(QuizHubDbContext).Assembly.FullName)
            ));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IOptionRepository, OptionRepository>();
        services.AddScoped<IQuizAttemptRepository, QuizAttemptRepository>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}