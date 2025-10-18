using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.DTOs.Option.CreateOption;
using QuizHub.Application.DTOs.Question.CreateQuestion;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Entities;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Question.CreateQuestion;

public sealed class CreateQuestionCommandHandler(
    IQuizRepository quizRepository,
    IQuestionRepository questionRepository,
    IOptionRepository optionRepository,
    IUnitOfWork uow
) : IRequestHandler<CreateQuestionCommand, Result<CreateQuestionResponseDto>>
{
    public async Task<Result<CreateQuestionResponseDto>> Handle(CreateQuestionCommand command, CancellationToken ct)
    {
        var req = command.Request;

        if (string.IsNullOrWhiteSpace(req.Text))
            return Result<CreateQuestionResponseDto>.Failure("Question text is required.");

        if (req.Points <= 0)
            return Result<CreateQuestionResponseDto>.Failure("Points must be greater than 0.");

        if (!Enum.TryParse<QuestionType>(req.Type, true, out var questionType))
            return Result<CreateQuestionResponseDto>.Failure("Invalid question type.");

        var quiz = await quizRepository.GetByIdAsync(req.QuizId, ct);
        if (quiz == null || quiz.IsDeleted)
            return Result<CreateQuestionResponseDto>.Failure("Quiz not found.");

        if (questionType == QuestionType.SingleChoice || questionType == QuestionType.MultipleChoice)
        {
            if (req.Options.Count < 4)
                return Result<CreateQuestionResponseDto>.Failure("Choice questions must have at least 4 options.");
            if (questionType == QuestionType.SingleChoice && req.Options.Count(o => o.IsCorrect) != 1)
                return Result<CreateQuestionResponseDto>.Failure("Single-choice questions must have exactly one correct option.");
            if (questionType == QuestionType.MultipleChoice && !req.Options.Any(o => o.IsCorrect))
                return Result<CreateQuestionResponseDto>.Failure("Multiple-choice questions must have at least one correct option.");
        }
        else if (questionType == QuestionType.FillInBlank || questionType == QuestionType.TrueFalse)
        {
            if (string.IsNullOrWhiteSpace(req.CorrectAnswer))
                return Result<CreateQuestionResponseDto>.Failure("Correct answer is required for FillInBlank or TrueFalse questions.");
        }

        var question = new Domain.Entities.Question(
            quizId: req.QuizId,
            text: req.Text,
            type: questionType,
            points: req.Points,
            correctAnswer: req.CorrectAnswer
        );

        await questionRepository.AddAsync(question, ct);

        var options = req.Options.Select(opt => new Option(question.Id, opt.Text, opt.IsCorrect)).ToList();
        foreach (var option in options)
        {
            await optionRepository.AddAsync(option, ct);
        }

        await uow.SaveChangesAsync(ct);

        return Result<CreateQuestionResponseDto>.Success(new CreateQuestionResponseDto
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
