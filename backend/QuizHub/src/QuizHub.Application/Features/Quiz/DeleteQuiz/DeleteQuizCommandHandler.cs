using MediatR;
using QuizHub.Application.DTOs.Common;
using QuizHub.Application.Interfaces;
using QuizHub.Domain.Repositories;

namespace QuizHub.Application.Features.Quiz.DeleteQuiz;

public sealed class DeleteQuizCommandHandler(
    IQuizRepository quizRepository,
    IUnitOfWork uow
) : IRequestHandler<DeleteQuizCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteQuizCommand command, CancellationToken ct)
    {
        var quiz = await quizRepository.GetByIdAsync(command.Id, ct);
        if (quiz == null || quiz.IsDeleted)
            return Result<Unit>.Failure("Quiz not found.");

        await quizRepository.DeleteAsync(command.Id, ct);
        await uow.SaveChangesAsync(ct);

        return Result<Unit>.Success(Unit.Value);
    }
}
