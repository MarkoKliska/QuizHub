using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.QuizAttempt.QuizAttemptDetails;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.QuizAttempt.GetQuizAttemptDetails;

public sealed class GetQuizAttemptDetailsQueryHandler(
    IQuizAttemptRepository quizAttemptRepository,
    IQuestionRepository questionRepository,
    IOptionRepository optionRepository
) : IRequestHandler<GetQuizAttemptDetailsQuery, Result<QuizAttemptDetailsDto>>
{
    public async Task<Result<QuizAttemptDetailsDto>> Handle(GetQuizAttemptDetailsQuery query, CancellationToken ct)
    {
        var attempt = await quizAttemptRepository.GetByIdAsync(query.QuizAttemptId, ct);
        if (attempt == null || !attempt.IsCompleted)
            return Result<QuizAttemptDetailsDto>.Failure("Attempt not found or not completed.");

        var quiz = attempt.Quiz;
        var questions = await questionRepository.GetByQuizIdAsync(quiz.Id, ct);

        var questionDtos = new List<QuestionDetailDto>();

        foreach (var question in questions)
        {
            var options = await optionRepository.GetByQuestionIdAsync(question.Id, ct);


            var optionDtos = options.Select(o => new OptionDetailDto
            {
                Text = o.Text,
                IsCorrect = o.IsCorrect,
                Selected = false
            }).ToList();

            questionDtos.Add(new QuestionDetailDto
            {
                Text = question.Text,
                Type = question.Type.ToString(),
                Options = optionDtos,
                IsCorrect = false 
            });
        }

        var result = new QuizAttemptDetailsDto
        {
            QuizName = quiz.Name,
            Score = attempt.Score ?? 0,
            Percentage = attempt.Percentage ?? 0,
            StartTime = attempt.StartTime,
            EndTime = attempt.EndTime ?? DateTime.UtcNow,
            Questions = questionDtos
        };

        return Result<QuizAttemptDetailsDto>.Success(result);
    }
}
