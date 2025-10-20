using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Option.CreateOption;
using QuizHub.Application.DTOs.Question.UpdateQuestion;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Question.UpdateQuestion;

public sealed class UpdateQuestionCommandHandler(
    IQuizRepository quizRepository,
    IQuestionRepository questionRepository,
    IOptionRepository optionRepository,
    IUnitOfWork uow
) : IRequestHandler<UpdateQuestionCommand, Result<UpdateQuestionResponseDto>>
{
    public async Task<Result<UpdateQuestionResponseDto>> Handle(UpdateQuestionCommand command, CancellationToken ct)
    {
        var req = command.Request;

        if (string.IsNullOrWhiteSpace(req.Text))
            return Result<UpdateQuestionResponseDto>.Failure("Question text is required.");

        if (req.Points <= 0)
            return Result<UpdateQuestionResponseDto>.Failure("Points must be greater than 0.");

        if (!Enum.TryParse<QuestionType>(req.Type, true, out var questionType))
            return Result<UpdateQuestionResponseDto>.Failure("Invalid question type.");

        var quiz = await quizRepository.GetByIdAsync(req.QuizId, ct);
        if (quiz == null || quiz.IsDeleted)
            return Result<UpdateQuestionResponseDto>.Failure("Quiz not found.");

        var question = await questionRepository.GetByIdAsync(command.Id, ct);
        if (question == null || question.IsDeleted)
            return Result<UpdateQuestionResponseDto>.Failure("Question not found.");

        if (questionType == QuestionType.SingleChoice || questionType == QuestionType.MultipleChoice)
        {
            if (req.Options.Count < 4)
                return Result<UpdateQuestionResponseDto>.Failure("Choice questions must have at least 4 options.");
            if (questionType == QuestionType.SingleChoice && req.Options.Count(o => o.IsCorrect) != 1)
                return Result<UpdateQuestionResponseDto>.Failure("Single-choice questions must have exactly one correct option.");
            if (questionType == QuestionType.MultipleChoice && !req.Options.Any(o => o.IsCorrect))
                return Result<UpdateQuestionResponseDto>.Failure("Multiple-choice questions must have at least one correct option.");
        }
        else if (questionType == QuestionType.FillInBlank || questionType == QuestionType.TrueFalse)
        {
            if (string.IsNullOrWhiteSpace(req.CorrectAnswer))
                return Result<UpdateQuestionResponseDto>.Failure("Correct answer is required for FillInBlank or TrueFalse questions.");
        }

        question.Update(req.Text, req.Points, req.CorrectAnswer);
        question.UpdateType(questionType);

        var existingOptions = await optionRepository.GetByQuestionIdAsync(question.Id, ct);
        foreach (var opt in existingOptions)
            await optionRepository.DeleteAsync(opt.Id, ct);

        var options = req.Options.Select(opt => new Option(question.Id, opt.Text, opt.IsCorrect)).ToList();
        foreach (var option in options)
        {
            await optionRepository.AddAsync(option, ct);
        }

        await questionRepository.UpdateAsync(question, ct);
        await uow.SaveChangesAsync(ct);

        return Result<UpdateQuestionResponseDto>.Success(new UpdateQuestionResponseDto
        {
            Id = question.Id,
            QuizId = question.QuizId,
            Text = question.Text,
            Type = question.Type.ToString(),
            Points = question.Points,
            CorrectAnswer = question.CorrectAnswer,
            Options = options.Select(o => new CreateOptionResponseDto
            {
                Id = o.Id,
                Text = o.Text,
                IsCorrect = o.IsCorrect
            }).ToList()
        });
    }
}
