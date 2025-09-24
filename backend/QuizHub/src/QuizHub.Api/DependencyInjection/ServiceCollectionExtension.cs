namespace QuizHub.Api.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
    {
        return services;
    }
}
