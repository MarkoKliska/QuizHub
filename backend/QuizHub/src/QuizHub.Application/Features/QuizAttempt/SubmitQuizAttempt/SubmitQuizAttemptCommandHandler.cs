using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.QuizAttempt.SubmitQuizAttempt;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.QuizAttempt.SubmitQuizAttempt;

public sealed class SubmitQuizAttemptCommandHandler(
    IQuizAttemptRepository quizAttemptRepository,
    IQuizRepository quizRepository,
    IQuestionRepository questionRepository,
    IOptionRepository optionRepository,
    IUnitOfWork uow
) : IRequestHandler<SubmitQuizAttemptCommand, Result<SubmitQuizAttemptResponseDto>>
{
    public async Task<Result<SubmitQuizAttemptResponseDto>> Handle(SubmitQuizAttemptCommand command, CancellationToken ct)
    {
        var req = command.Request;

        var attempt = await quizAttemptRepository.GetByIdAsync(req.QuizAttemptId, ct);
        if (attempt == null || attempt.IsCompleted)
            return Result<SubmitQuizAttemptResponseDto>.Failure("Quiz attempt not found or already completed.");

        if (attempt.UserId != command.UserId)
            return Result<SubmitQuizAttemptResponseDto>.Failure("Unauthorized: User does not own this attempt.");

        var quiz = await quizRepository.GetByIdAsync(attempt.QuizId, ct);
        if (quiz == null || quiz.IsDeleted)
            return Result<SubmitQuizAttemptResponseDto>.Failure("Quiz not found.");

        var elapsedTime = DateTime.UtcNow - attempt.StartTime;
        if (elapsedTime.TotalMinutes > quiz.TimeLimit)
            return Result<SubmitQuizAttemptResponseDto>.Failure("Time limit exceeded.");

        var questions = await questionRepository.GetByQuizIdAsync(quiz.Id, ct);
        if (!questions.Any())
            return Result<SubmitQuizAttemptResponseDto>.Failure("Quiz has no questions.");

        int totalPoints = questions.Sum(q => q.Points);
        int score = 0;

        foreach (var answer in req.Answers)
        {
            var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question == null || question.IsDeleted)
                continue;

            bool isCorrect = false;
            switch (question.Type)
            {
                case QuestionType.SingleChoice:
                case QuestionType.MultipleChoice:
                    var options = await optionRepository.GetByQuestionIdAsync(question.Id, ct);
                    var correctOptionIds = options.Where(o => o.IsCorrect).Select(o => o.Id).ToList();
                    var selectedOptionIds = answer.SelectedOptionIds ?? new List<Guid>();

                    if (question.Type == QuestionType.SingleChoice)
                    {
                        isCorrect = selectedOptionIds.Count == 1 && correctOptionIds.Count == 1 && selectedOptionIds[0] == correctOptionIds[0];
                    }
                    else
                    {
                        isCorrect = selectedOptionIds.Count == correctOptionIds.Count && selectedOptionIds.All(id => correctOptionIds.Contains(id));
                    }
                    break;

                case QuestionType.TrueFalse:
                case QuestionType.FillInBlank:
                    isCorrect = string.Equals(answer.TextAnswer?.Trim(), question.CorrectAnswer?.Trim(), StringComparison.OrdinalIgnoreCase);
                    break;
            }

            if (isCorrect)
                score += question.Points;
        }

        double percentage = totalPoints > 0 ? (score / (double)totalPoints) * 100 : 0;

        attempt.Complete(score, percentage);

        await quizAttemptRepository.UpdateAsync(attempt, ct);
        await uow.SaveChangesAsync(ct);

        return Result<SubmitQuizAttemptResponseDto>.Success(new SubmitQuizAttemptResponseDto
        {
            Id = attempt.Id,
            UserId = attempt.UserId,
            Username = attempt.User.UserName,
            QuizId = attempt.QuizId,
            QuizName = quiz.Name,
            StartTime = attempt.StartTime,
            EndTime = attempt.EndTime,
            Score = attempt.Score!.Value,
            Percentage = attempt.Percentage!.Value
        });
    }
}
