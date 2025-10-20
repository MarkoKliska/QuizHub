using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Option.CreateOption;
using QuizHub.Application.DTOs.Question.GetQuestionByQuizId;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Question.GetQuestionByQuizId;

public sealed class GetQuestionsByQuizIdQueryHandler(IQuestionRepository questionRepository)
    : IRequestHandler<GetQuestionsByQuizIdQuery, Result<IEnumerable<GetQuestionByQuizIdResponseDto>>>
{
    public async Task<Result<IEnumerable<GetQuestionByQuizIdResponseDto>>> Handle(GetQuestionsByQuizIdQuery query, CancellationToken ct)
    {
        var questions = await questionRepository.GetByQuizIdAsync(query.QuizId, ct);
        var response = questions.Select(q => new GetQuestionByQuizIdResponseDto
        {
            Id = q.Id,
            QuizId = q.QuizId,
            Text = q.Text,
            Type = q.Type.ToString(),
            Points = q.Points,
            CorrectAnswer = q.CorrectAnswer,
            Options = q.Options.Select(o => new CreateOptionResponseDto
            {
                Id = o.Id,
                Text = o.Text,
                IsCorrect = o.IsCorrect
            }).ToList()
        });

        return Result<IEnumerable<GetQuestionByQuizIdResponseDto>>.Success(response);
    }
}
