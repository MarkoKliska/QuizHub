using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizHub.Application.DTOs.Quiz.GetFilteredQuizzes;
using QuizHub.Application.DTOs.QuizAttempt.StartQuizAttempt;
using QuizHub.Application.DTOs.QuizAttempt.SubmitQuizAttempt;
using QuizHub.Application.Features.Quiz.GetFilteredQuizzes;
using QuizHub.Application.Features.QuizAttempt.GetMyResults;
using QuizHub.Application.Features.QuizAttempt.GetQuizAttemptDetails;
using QuizHub.Application.Features.QuizAttempt.StartQuizAttempt;
using QuizHub.Application.Features.QuizAttempt.SubmitQuizAttempt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QuizHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost("attempts")]
    [Authorize]
    public async Task<IActionResult> StartQuizAttempt([FromBody] StartQuizAttemptRequestDto request, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var result = await mediator.Send(new StartQuizAttemptCommand(request, userId), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpPost("attempts/submit")]
    [Authorize]
    public async Task<IActionResult> SubmitQuizAttempt([FromBody] SubmitQuizAttemptRequestDto request, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                        User.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
        var result = await mediator.Send(new SubmitQuizAttemptCommand(request, userId), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetFilteredQuizzes(
        [FromQuery] Guid? categoryId,
        [FromQuery] string? difficulty,
        [FromQuery] string? search,
        CancellationToken ct)
    {
        var request = new GetFilteredQuizzesRequestDto
        {
            CategoryId = categoryId,
            Difficulty = difficulty,
            Search = search
        };

        var result = await mediator.Send(new GetFilteredQuizzesQuery(request), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet("attempts/user")]
    [Authorize]
    public async Task<IActionResult> GetUserQuizAttempts(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await mediator.Send(new GetMyResultsQuery(userId), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet("attempts/{attemptId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetQuizAttemptDetails(Guid attemptId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetQuizAttemptDetailsQuery(attemptId), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }
}
