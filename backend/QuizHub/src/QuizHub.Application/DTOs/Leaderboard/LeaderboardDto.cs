namespace QuizHub.Application.DTOs.Leaderboard;

public class GetLeaderboardRequestDto
{
    public Guid? QuizId { get; set; }
    public string? TimePeriod { get; set; }
}

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public Guid QuizId { get; set; }
    public string QuizName { get; set; } = string.Empty;
    public int Score { get; set; }
    public double Percentage { get; set; }
    public DateTime CompletedAt { get; set; }
    public int DurationSeconds { get; set; }
    public bool IsCurrentUser { get; set; }
}

public class LeaderboardResponseDto
{
    public List<LeaderboardEntryDto> Entries { get; set; } = new();
    public LeaderboardEntryDto? CurrentUserEntry { get; set; }
    public int? CurrentUserRank { get; set; }
    public int TotalEntries { get; set; }
}
