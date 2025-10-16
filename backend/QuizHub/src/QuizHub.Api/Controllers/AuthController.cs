using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizHub.Application.DTOs.User.Login;
using QuizHub.Application.DTOs.User.RegisterUser;
using QuizHub.Application.Features.User.Login;
using QuizHub.Application.Features.User.RegisterUser;

namespace QuizHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request, CancellationToken ct)
    {
        var result = await mediator.Send(new RegisterUserCommand(request), ct);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken ct)
    {
        var result = await mediator.Send(new LoginCommand(request), ct);

        if(!result.IsSuccess)
            return Unauthorized(new {error = result.Error});

        return Ok(result.Value);
    }
}
