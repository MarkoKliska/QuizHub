using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Leaderboard;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Leaderboard.GetLeaderboard;

public sealed class GetLeaderboardQueryHandler(
    IQuizAttemptRepository quizAttemptRepository
) : IRequestHandler<GetLeaderboardQuery, Result<LeaderboardResponseDto>>
{
    public async Task<Result<LeaderboardResponseDto>> Handle(GetLeaderboardQuery query, CancellationToken ct)
    {
        var req = query.Request;

        var allAttempts = await quizAttemptRepository.GetAllAsync(ct);
        var completedAttempts = allAttempts.Where(a => a.IsCompleted && a.Score.HasValue).ToList();

        if (req.QuizId.HasValue)
        {
            completedAttempts = completedAttempts.Where(a => a.QuizId == req.QuizId.Value).ToList();
        }

        DateTime? startDate = req.TimePeriod?.ToLower() switch
        {
            "weekly" => DateTime.UtcNow.AddDays(-7),
            "monthly" => DateTime.UtcNow.AddMonths(-1),
            _ => null
        };

        if (startDate.HasValue)
        {
            completedAttempts = completedAttempts
                .Where(a => a.EndTime.HasValue && a.EndTime.Value >= startDate.Value)
                .ToList();
        }

        var bestAttempts = completedAttempts
            .GroupBy(a => new { a.UserId, a.QuizId })
            .Select(g => g.OrderByDescending(a => a.Score)
                          .ThenByDescending(a => a.Percentage)
                          .ThenBy(a => a.EndTime)
                          .First())
            .ToList();

        var sortedAttempts = bestAttempts
            .OrderByDescending(a => a.Score)
            .ThenByDescending(a => a.Percentage)
            .ThenBy(a => a.EndTime)
            .ToList();

        var entries = new List<LeaderboardEntryDto>();
        for (int i = 0; i < sortedAttempts.Count; i++)
        {
            var attempt = sortedAttempts[i];
            var durationSeconds = attempt.EndTime.HasValue
                ? (int)(attempt.EndTime.Value - attempt.StartTime).TotalSeconds
                : 0;

            entries.Add(new LeaderboardEntryDto
            {
                Rank = i + 1,
                UserId = attempt.UserId,
                Username = attempt.User?.UserName ?? "Unknown",
                QuizId = attempt.QuizId,
                QuizName = attempt.Quiz?.Name ?? "Unknown Quiz",
                Score = attempt.Score!.Value,
                Percentage = attempt.Percentage!.Value,
                CompletedAt = attempt.EndTime ?? DateTime.UtcNow,
                DurationSeconds = durationSeconds,
                IsCurrentUser = attempt.UserId == query.CurrentUserId
            });
        }

        var currentUserEntry = entries.FirstOrDefault(e => e.IsCurrentUser);
        var currentUserRank = currentUserEntry?.Rank;

        var response = new LeaderboardResponseDto
        {
            Entries = entries.Take(100).ToList(), 
            CurrentUserEntry = currentUserEntry,
            CurrentUserRank = currentUserRank,
            TotalEntries = entries.Count
        };

        return Result<LeaderboardResponseDto>.Success(response);
    }
}
