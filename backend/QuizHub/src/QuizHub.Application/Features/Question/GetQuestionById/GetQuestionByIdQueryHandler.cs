using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Option.CreateOption;
using QuizHub.Application.DTOs.Question.GetQuestion;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Question.GetQuestionById;

public sealed class GetQuestionByIdQueryHandler(IQuestionRepository questionRepository)
    : IRequestHandler<GetQuestionByIdQuery, Result<GetQuestionByIdResponseDto>>
{
    public async Task<Result<GetQuestionByIdResponseDto>> Handle(GetQuestionByIdQuery query, CancellationToken ct)
    {
        var question = await questionRepository.GetByIdAsync(query.Id, ct);
        if (question == null || question.IsDeleted)
            return Result<GetQuestionByIdResponseDto>.Failure("Question not found.");

        return Result<GetQuestionByIdResponseDto>.Success(new GetQuestionByIdResponseDto
        {
            Id = question.Id,
            QuizId = question.QuizId,
            Text = question.Text,
            Type = question.Type.ToString(),
            Points = question.Points,
            CorrectAnswer = question.CorrectAnswer,
            Options = question.Options.Select(o => new CreateOptionResponseDto
            {
                Id = o.Id,
                Text = o.Text,
                IsCorrect = o.IsCorrect
            }).ToList()
        });
    }
}
