using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace QuizHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController(
    IMediator mediator
) : ControllerBase
{

}
