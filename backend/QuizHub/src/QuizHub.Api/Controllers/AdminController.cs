using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizHub.Application.DTOs.Category.CreateCategory;
using QuizHub.Application.DTOs.Question.CreateQuestion;
using QuizHub.Application.DTOs.Quiz.CreateQuiz;
using QuizHub.Application.Features.Category.CreateCategory;
using QuizHub.Application.Features.Category.DeleteCategory;
using QuizHub.Application.Features.Category.GetAllCategories;
using QuizHub.Application.Features.Category.GetCategoryById;
using QuizHub.Application.Features.Category.UpdateCategory;
using QuizHub.Application.Features.Question.CreateQuestion;
using QuizHub.Application.Features.Question.DeleteQuestion;
using QuizHub.Application.Features.Question.GetQuestionById;
using QuizHub.Application.Features.Question.GetQuestionByQuizId;
using QuizHub.Application.Features.Question.UpdateQuestion;
using QuizHub.Application.Features.Quiz.CreateQuiz;
using QuizHub.Application.Features.Quiz.DeleteQuiz;
using QuizHub.Application.Features.Quiz.GetAllQuizzes;
using QuizHub.Application.Features.Quiz.GetQuizById;
using QuizHub.Application.Features.Quiz.UpdateQuiz;
using System.Security.Claims;

namespace QuizHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class QuizAdminController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateCategoryCommand(request), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet("categories/{id}")]
    public async Task<IActionResult> GetCategory(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCategoryByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllCategoriesQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpPut("categories/{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CreateCategoryRequestDto request, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateCategoryCommand(id, request), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpDelete("categories/{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteCategoryCommand(id), ct);
        return result.IsSuccess ? NoContent() : NotFound(new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequestDto request, CancellationToken ct)
    {
        var adminId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        var result = await mediator.Send(new CreateQuizCommand(request, adminId), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuiz(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetQuizByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllQuizzes(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllQuizzesQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuiz(Guid id, [FromBody] CreateQuizRequestDto request, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateQuizCommand(id, request), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuiz(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteQuizCommand(id), ct);
        return result.IsSuccess ? NoContent() : NotFound(new { error = result.Error });
    }

    [HttpPost("questions")]
    public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequestDto request, CancellationToken ct)
    {
        var result = await mediator.Send(new CreateQuestionCommand(request), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet("questions/{id}")]
    public async Task<IActionResult> GetQuestion(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetQuestionByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
    }

    [HttpGet("quizzes/{quizId}/questions")]
    public async Task<IActionResult> GetQuestionsByQuizId(Guid quizId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetQuestionsByQuizIdQuery(quizId), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpPut("questions/{id}")]
    public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] CreateQuestionRequestDto request, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateQuestionCommand(id, request), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpDelete("questions/{id}")]
    public async Task<IActionResult> DeleteQuestion(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteQuestionCommand(id), ct);
        return result.IsSuccess ? NoContent() : NotFound(new { error = result.Error });
    }
}
