using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizHub.Application.DTOs.QuizAttempt.StartQuizAttempt;
using QuizHub.Application.DTOs.QuizAttempt.SubmitQuizAttempt;
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
}
