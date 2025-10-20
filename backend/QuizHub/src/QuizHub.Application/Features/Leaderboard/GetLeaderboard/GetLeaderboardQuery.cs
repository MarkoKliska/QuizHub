using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Leaderboard;

namespace QuizHub.Application.Features.Leaderboard.GetLeaderboard;

public sealed record GetLeaderboardQuery(
    GetLeaderboardRequestDto Request,
    Guid CurrentUserId
) : IRequest<Result<LeaderboardResponseDto>>;
